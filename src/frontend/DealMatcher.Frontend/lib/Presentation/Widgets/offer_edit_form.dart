import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:frontend/Models/category.dart';
import 'package:frontend/Models/category_property.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Services/category_service.dart';
import 'package:frontend/Services/offer_service.dart';
import 'package:go_router/go_router.dart';

class OfferEditForm extends StatefulWidget {
  final int offerId;

  const OfferEditForm({super.key, required this.offerId});

  @override
  State<OfferEditForm> createState() => _OfferEditFormState();
}

class _OfferEditFormState extends State<OfferEditForm> {
  final OfferService _offerService = OfferService();
  final CategoryService _categoryService = CategoryService();
  final _formKey = GlobalKey<FormState>();

  bool _isLoading = true;
  String? _loadError;

  final _titleController = TextEditingController();
  final _descriptionController = TextEditingController();
  final _priceController = TextEditingController();
  final _availabilityController = TextEditingController();

  final List<TextEditingController> _tagsControllers = [
    TextEditingController(),
  ];

  // Existing images from API (URLs the user can remove)
  List<String> _existingImageUrls = [];
  final List<String> _imagesToRemove = [];

  List<Category> _categories = [];
  Category? _selectedCategory;
  List<CategoryProperty> _categoryProperties = [];
  final Map<String, dynamic> _propertyValues = {};

  bool _isSaving = false;
  bool _isDeleting = false;

  @override
  void initState() {
    super.initState();
    _loadInitialData();
  }

  /// Fetches the offer and the category list in parallel, then pre-fills
  /// all controllers and loads category-specific properties.
  Future<void> _loadInitialData() async {
    try {
      final results = await Future.wait([
        _offerService.getOffer(widget.offerId),
        _categoryService.getCategories(),
      ]);

      final offer = results[0] as Offer;
      final categories = results[1] as List<Category>;

      // Match offer category to the fetched list by id
      final matchedCategory = categories.cast<Category?>().firstWhere(
        (c) => c?.id == offer.category.id,
        orElse: () => null,
      );

      // Basic controllers
      _titleController.text = offer.title;
      _descriptionController.text = offer.description;
      _priceController.text = offer.price.toStringAsFixed(2);
      _availabilityController.text = offer.availability.toString();
      _existingImageUrls = List<String>.from(offer.images);

      // Tags — replace the single default controller
      for (final c in _tagsControllers) {
        c.dispose();
      }
      _tagsControllers.clear();
      if (offer.tags.isEmpty) {
        _tagsControllers.add(TextEditingController());
      } else {
        for (final tag in offer.tags) {
          _tagsControllers.add(TextEditingController(text: tag));
        }
      }

      setState(() {
        _categories = categories;
        _selectedCategory = matchedCategory;
        _isLoading = false;
      });

      // Load properties for the matched category and pre-fill values
      if (matchedCategory != null) {
        await _loadProperties(
          matchedCategory,
          existingProperties: offer.properties,
        );
      }
    } catch (e) {
      setState(() {
        _loadError = e.toString();
        _isLoading = false;
      });
    }
  }

  /// Loads category properties and optionally pre-fills values from the offer.
  Future<void> _loadProperties(
    Category category, {
    Map<String, String>? existingProperties,
  }) async {
    setState(() {
      _categoryProperties = [];
      _propertyValues.clear();
    });

    try {
      final properties = await _categoryService.getCategoryProperties(
        category.name,
      );

      // Build a lookup map from existing offer properties
      final existingMap = existingProperties ?? <String, String>{};

      setState(() {
        _categoryProperties = properties;
        for (final property in properties) {
          final key = property.id.toString();
          final existing = existingMap[key];
          switch (property.type) {
            case "BOOLEAN":
              _propertyValues[key] = existing == 'true';
              break;
            case "SELECT":
              final firstOption = property.options.isNotEmpty
                  ? property.options.first
                  : null;
              _propertyValues[key] = existing ?? firstOption;
              break;
            default: // text  / numeric
              _propertyValues[key] = existing ?? '';
              break;
          }
        }
      });
    } catch (e) {
      debugPrint('Błąd ładowania właściwości kategorii: $e');
    }
  }

  @override
  void dispose() {
    _titleController.dispose();
    _descriptionController.dispose();
    _priceController.dispose();
    _availabilityController.dispose();
    for (final c in _tagsControllers) {
      c.dispose();
    }
    super.dispose();
  }

  int get _totalImages => _existingImageUrls.length;

  void _removeExistingImage(int index) {
    if (_existingImageUrls.length <= 1) return;

    setState(() {
      _imagesToRemove.add(_existingImageUrls[index]);
      _existingImageUrls.removeAt(index);
    });
  }

  void _addTag() {
    if (_tagsControllers.length >= 10) return;
    setState(() => _tagsControllers.add(TextEditingController()));
  }

  void _removeTag() {
    if (_tagsControllers.length <= 1) return;
    setState(() => _tagsControllers.removeLast().dispose());
  }

  Widget _buildPropertyField(CategoryProperty property) {
    final key = property.id.toString();
    switch (property.type) {
      case "TEXT": // text
        return _PropPad(
          child: TextFormField(
            initialValue: (_propertyValues[key] ?? '').toString(),
            decoration: _decoration(context, property.name),
            style: const TextStyle(color: Colors.white),
            autovalidateMode: AutovalidateMode.onUserInteraction,
            onChanged: (v) => _propertyValues[key] = v,
            validator: (v) => (v == null || v.trim().isEmpty)
                ? 'Wymagana jest wartość właściwości'
                : null,
          ),
        );
      case "NUMBER": // numeric
        return _PropPad(
          child: TextFormField(
            initialValue: (_propertyValues[key] ?? '').toString(),
            decoration: _decoration(context, property.name),
            style: const TextStyle(color: Colors.white),
            keyboardType: TextInputType.number,
            autovalidateMode: AutovalidateMode.onUserInteraction,
            onChanged: (v) => _propertyValues[key] = v,
            validator: (v) {
              if (v == null || v.trim().isEmpty) {
                return 'Wymagana jest wartość właściwości';
              }
              if (num.tryParse(v) == null) return 'Wartość musi być liczbą';
              return null;
            },
          ),
        );
      case "BOOLEAN": // boolean
        return _PropPad(
          child: SwitchListTile(
            title: Text(
              property.name,
              style: const TextStyle(color: Colors.white70),
            ),
            value: (_propertyValues[key] as bool?) ?? false,
            onChanged: (v) => setState(() => _propertyValues[key] = v),
            contentPadding: EdgeInsets.zero,
          ),
        );
      case "SELECT": // select
        return _PropPad(
          child: DropdownButtonFormField<String>(
            initialValue: _propertyValues[key] as String?,
            decoration: _decoration(context, property.name),
            dropdownColor: const Color.fromARGB(255, 48, 46, 44),
            style: const TextStyle(color: Colors.white),
            items: property.options
                .map((o) => DropdownMenuItem(value: o, child: Text(o)))
                .toList(),
            onChanged: (v) => setState(() => _propertyValues[key] = v),
            validator: (v) => (v == null || v.isEmpty) ? 'Wybierz opcję' : null,
          ),
        );
      default:
        return _PropPad(
          child: Text(
            'Nieobsługiwany typ pola: ${property.type}',
            style: const TextStyle(color: Colors.redAccent),
          ),
        );
    }
  }

  InputDecoration _decoration(
    BuildContext context,
    String label, {
    String? hint,
  }) {
    final primary = Theme.of(context).primaryColor;
    return InputDecoration(
      labelText: label,
      hintText: hint,
      filled: true,
      fillColor: Colors.white.withValues(alpha: 0.04),
      labelStyle: const TextStyle(color: Colors.white54),
      hintStyle: const TextStyle(color: Colors.white30),
      enabledBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(14),
        borderSide: BorderSide(color: Colors.white.withValues(alpha: 0.10)),
      ),
      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(14),
        borderSide: BorderSide(color: primary.withValues(alpha: 0.55)),
      ),
      errorBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(14),
        borderSide: const BorderSide(color: Colors.redAccent),
      ),
      focusedErrorBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(14),
        borderSide: const BorderSide(color: Colors.redAccent),
      ),
    );
  }

  Widget _sectionTitle(BuildContext context, String title) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 10),
      child: Text(
        title,
        style: TextStyle(
          color: Theme.of(context).primaryColor,
          fontSize: 12,
          fontWeight: FontWeight.w700,
          letterSpacing: 0.8,
        ),
      ),
    );
  }

  Widget _card(BuildContext context, {required Widget child}) {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white.withValues(alpha: 0.04),
        borderRadius: BorderRadius.circular(22),
        border: Border.all(color: Colors.white.withValues(alpha: 0.08)),
      ),
      padding: const EdgeInsets.all(18),
      child: child,
    );
  }

  Future<void> _save() async {
    if (!(_formKey.currentState?.validate() ?? false)) return;
    setState(() => _isSaving = true);

    try {
      final tags = _tagsControllers
          .map((c) => c.text.trim())
          .where((t) => t.isNotEmpty)
          .toList();

      final properties = <String, String>{
        for (final p in _categoryProperties)
          p.id.toString(): _propertyValues[p.id.toString()]?.toString() ?? '',
      };

      final data = {
        'title': _titleController.text.trim(),
        'description': _descriptionController.text.trim(),
        'price': double.parse(_priceController.text.trim()),
        'categoryId': _selectedCategory?.id,
        'availability': int.parse(_availabilityController.text.trim()),
        'tags': tags,
        'properties': properties,
        // Existing images to keep are sent as part of the JSON payload
        'images': _imagesToRemove,
      };

      await _offerService.updateOffer(widget.offerId, data);

      if (!mounted) return;
      context.go('/my-offers');
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Nie udało się zapisać oferty: $e')),
      );
    } finally {
      if (mounted) setState(() => _isSaving = false);
    }
  }

  Future<void> _confirmAndDelete() async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (ctx) => AlertDialog(
        backgroundColor: const Color.fromARGB(255, 32, 30, 28),
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        title: Row(
          children: [
            Container(
              padding: const EdgeInsets.all(8),
              decoration: BoxDecoration(
                color: Colors.red.withValues(alpha: 0.15),
                borderRadius: BorderRadius.circular(10),
              ),
              child: const Icon(
                Icons.delete_forever_outlined,
                color: Colors.redAccent,
                size: 22,
              ),
            ),
            const SizedBox(width: 12),
            const Text(
              'Usuń ofertę',
              style: TextStyle(color: Colors.white, fontSize: 18),
            ),
          ],
        ),
        content: const Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Czy na pewno chcesz usunąć tę ofertę?',
              style: TextStyle(
                color: Colors.white,
                fontWeight: FontWeight.w600,
              ),
            ),
            SizedBox(height: 10),
            Row(
              children: [
                Icon(
                  Icons.warning_amber_rounded,
                  color: Colors.orangeAccent,
                  size: 16,
                ),
                SizedBox(width: 6),
                Expanded(
                  child: Text(
                    'Ta operacja jest nieodwracalna.',
                    style: TextStyle(color: Colors.white60, fontSize: 13),
                  ),
                ),
              ],
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx, false),
            child: const Text(
              'Anuluj',
              style: TextStyle(color: Colors.white54),
            ),
          ),
          ElevatedButton(
            onPressed: () => Navigator.pop(ctx, true),
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.redAccent,
              foregroundColor: Colors.white,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(10),
              ),
            ),
            child: const Text('Tak, usuń'),
          ),
        ],
      ),
    );

    if (confirmed != true || !mounted) return;

    setState(() => _isDeleting = true);
    try {
      await _offerService.deleteOffer(widget.offerId);
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Oferta została usunięta.')));
      context.go('/my-offers');
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Nie udało się usunąć oferty: $e')),
      );
    } finally {
      if (mounted) setState(() => _isDeleting = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    // Loading / error states
    if (_isLoading) {
      return const Center(child: CircularProgressIndicator());
    }

    if (_loadError != null) {
      return Center(
        child: Container(
          constraints: const BoxConstraints(maxWidth: 560),
          padding: const EdgeInsets.all(24),
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(22),
            color: Colors.white.withValues(alpha: 0.04),
            border: Border.all(color: Colors.white.withValues(alpha: 0.08)),
          ),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              const Icon(Icons.error_outline, size: 48, color: Colors.white70),
              const SizedBox(height: 12),
              Text(
                'Nie udało się wczytać oferty',
                style: Theme.of(
                  context,
                ).textTheme.titleLarge?.copyWith(color: Colors.white),
              ),
              const SizedBox(height: 8),
              Text(
                _loadError!,
                style: const TextStyle(color: Colors.white70),
                textAlign: TextAlign.center,
              ),
            ],
          ),
        ),
      );
    }

    final primary = Theme.of(context).primaryColor;
    final busy = _isSaving || _isDeleting;

    return Form(
      key: _formKey,
      child: ListView(
        children: [
          Container(
            decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(24),
              gradient: LinearGradient(
                colors: [
                  primary.withValues(alpha: 0.20),
                  const Color.fromARGB(255, 28, 28, 28),
                ],
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              border: Border.all(color: primary.withValues(alpha: 0.25)),
            ),
            padding: const EdgeInsets.all(20),
            child: Row(
              children: [
                CircleAvatar(
                  radius: 28,
                  backgroundColor: primary,
                  child: const Icon(Icons.edit_outlined, color: Colors.white),
                ),
                const SizedBox(width: 16),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Edycja oferty',
                        style: Theme.of(context).textTheme.titleLarge?.copyWith(
                          color: Colors.white,
                          fontSize: 24,
                        ),
                      ),
                      const SizedBox(height: 4),
                      const Text(
                        'Zmień szczegóły swojej oferty. Pamiętaj, aby zapisać zmiany.',
                        style: TextStyle(color: Colors.white70),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),

          const SizedBox(height: 18),

          _card(
            context,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                _sectionTitle(context, 'DANE PODSTAWOWE'),
                TextFormField(
                  controller: _titleController,
                  decoration: _decoration(context, 'Tytuł'),
                  style: const TextStyle(color: Colors.white),
                  autovalidateMode: AutovalidateMode.onUserInteraction,
                  validator: (v) => (v == null || v.trim().isEmpty)
                      ? 'Wpisz tytuł oferty'
                      : null,
                ),
                const SizedBox(height: 12),
                TextFormField(
                  controller: _descriptionController,
                  decoration: _decoration(context, 'Opis'),
                  style: const TextStyle(color: Colors.white),
                  maxLines: 5,
                  minLines: 3,
                  autovalidateMode: AutovalidateMode.onUserInteraction,
                  validator: (v) => (v == null || v.trim().isEmpty)
                      ? 'Wpisz opis oferty'
                      : null,
                ),
                const SizedBox(height: 12),
                TextFormField(
                  controller: _priceController,
                  decoration: _decoration(
                    context,
                    'Cena (zł)',
                    hint: 'np. 49.99',
                  ),
                  style: const TextStyle(color: Colors.white),
                  keyboardType: const TextInputType.numberWithOptions(
                    decimal: true,
                  ),
                  inputFormatters: [
                    FilteringTextInputFormatter.allow(
                      RegExp(r'^\d*\.?\d{0,2}'),
                    ),
                  ],
                  autovalidateMode: AutovalidateMode.onUserInteraction,
                  validator: (v) {
                    final t = (v ?? '').trim();
                    if (t.isEmpty) return 'Podaj cenę';
                    final p = double.tryParse(t);
                    if (p == null) return 'Cena musi być liczbą (np. 49.99)';
                    if (p < 0) return 'Cena nie może być ujemna';
                    return null;
                  },
                ),
                const SizedBox(height: 12),
                TextFormField(
                  controller: _availabilityController,
                  decoration: _decoration(context, 'Dostępna ilość'),
                  style: const TextStyle(color: Colors.white),
                  keyboardType: TextInputType.number,
                  inputFormatters: [FilteringTextInputFormatter.digitsOnly],
                  autovalidateMode: AutovalidateMode.onUserInteraction,
                  validator: (v) {
                    final p = int.tryParse((v ?? '').trim());
                    if (p == null) {
                      return 'Podaj poprawną ilość (liczba całkowita)';
                    }
                    if (p < 0) return 'Ilość nie może być ujemna';
                    return null;
                  },
                ),
              ],
            ),
          ),

          const SizedBox(height: 14),

          _card(
            context,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                _sectionTitle(context, 'ZDJĘCIA'),
                if (_totalImages == 0)
                  Padding(
                    padding: const EdgeInsets.symmetric(vertical: 8),
                    child: Text(
                      'Brak zdjęć — dodaj co najmniej jedno',
                      style: TextStyle(
                        color: Colors.white.withValues(alpha: 0.35),
                        fontSize: 13,
                      ),
                    ),
                  ),
                Wrap(
                  spacing: 8,
                  runSpacing: 8,
                  children: [
                    // Existing images (URL previews)
                    ...List.generate(_existingImageUrls.length, (i) {
                      return _ImageThumb(
                        child: Image.network(
                          _existingImageUrls[i],
                          width: 80,
                          height: 80,
                          fit: BoxFit.cover,
                          errorBuilder: (_, __, ___) => Container(
                            width: 80,
                            height: 80,
                            color: Colors.white.withValues(alpha: 0.07),
                            child: const Icon(
                              Icons.broken_image_outlined,
                              color: Colors.white38,
                            ),
                          ),
                        ),
                        onRemove: () => _existingImageUrls.length > 1
                            ? () => _removeExistingImage(i)
                            : null,
                      );
                    }),
                  ],
                ),
              ],
            ),
          ),

          const SizedBox(height: 14),

          _card(
            context,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                _sectionTitle(context, 'KATEGORIA'),
                DropdownButtonFormField<Category>(
                  initialValue: _selectedCategory,
                  decoration: _decoration(context, 'Kategoria'),
                  dropdownColor: const Color.fromARGB(255, 48, 46, 44),
                  style: const TextStyle(color: Colors.white),
                  items: _categories
                      .map(
                        (c) => DropdownMenuItem(value: c, child: Text(c.name)),
                      )
                      .toList(),
                  onChanged: (c) async {
                    setState(() => _selectedCategory = c);
                    if (c != null) await _loadProperties(c);
                  },
                  validator: (v) => v == null ? 'Wybierz kategorię' : null,
                ),
                if (_categoryProperties.isNotEmpty) ...[
                  const SizedBox(height: 16),
                  _sectionTitle(context, 'WŁAŚCIWOŚCI KATEGORII'),
                  ..._categoryProperties.map(_buildPropertyField),
                ],
              ],
            ),
          ),

          const SizedBox(height: 14),

          _card(
            context,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                _sectionTitle(context, 'TAGI'),
                ...List.generate(_tagsControllers.length, (i) {
                  return Padding(
                    padding: EdgeInsets.only(
                      bottom: i < _tagsControllers.length - 1 ? 12 : 0,
                    ),
                    child: TextFormField(
                      controller: _tagsControllers[i],
                      decoration: _decoration(context, 'Tag ${i + 1}'),
                      style: const TextStyle(color: Colors.white),
                      autovalidateMode: AutovalidateMode.onUserInteraction,
                      validator: (v) =>
                          (v == null || v.trim().isEmpty) ? 'Wpisz tag' : null,
                    ),
                  );
                }),
                const SizedBox(height: 12),
                Row(
                  children: [
                    if (_tagsControllers.length > 1)
                      IconButton(
                        onPressed: _removeTag,
                        icon: const Icon(
                          Icons.remove_circle_outline,
                          color: Colors.white54,
                        ),
                        tooltip: 'Usuń tag',
                      ),
                    if (_tagsControllers.length < 10)
                      IconButton(
                        onPressed: _addTag,
                        icon: Icon(Icons.add_circle_outline, color: primary),
                        tooltip: 'Dodaj tag',
                      ),
                    const Spacer(),
                    Text(
                      '${_tagsControllers.length}/10',
                      style: TextStyle(
                        color: Colors.white.withValues(alpha: 0.35),
                        fontSize: 12,
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),

          const SizedBox(height: 18),

          SizedBox(
            width: double.infinity,
            child: ElevatedButton(
              onPressed: busy ? null : _save,
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 14),
                backgroundColor: primary,
                foregroundColor: Colors.white,
                disabledBackgroundColor: Colors.white12,
                disabledForegroundColor: Colors.white30,
              ),
              child: _isSaving
                  ? const SizedBox(
                      height: 18,
                      width: 18,
                      child: CircularProgressIndicator(strokeWidth: 2),
                    )
                  : const Text(
                      'Zapisz zmiany',
                      style: TextStyle(fontWeight: FontWeight.w700),
                    ),
            ),
          ),
          const SizedBox(height: 10),

          SizedBox(
            width: double.infinity,
            child: OutlinedButton.icon(
              onPressed: busy ? null : () => context.go('/my-offers'),
              icon: const Icon(Icons.arrow_back, size: 18),
              label: const Text('Wróć do listy'),
              style: OutlinedButton.styleFrom(
                foregroundColor: Colors.white70,
                side: const BorderSide(color: Colors.white24),
                padding: const EdgeInsets.symmetric(vertical: 12),
              ),
            ),
          ),
          const SizedBox(height: 10),

          SizedBox(
            width: double.infinity,
            child: OutlinedButton.icon(
              onPressed: busy ? null : _confirmAndDelete,
              icon: _isDeleting
                  ? const SizedBox(
                      width: 14,
                      height: 14,
                      child: CircularProgressIndicator(
                        strokeWidth: 2,
                        color: Colors.redAccent,
                      ),
                    )
                  : const Icon(Icons.delete_outline, size: 18),
              label: Text(_isDeleting ? 'Usuwanie…' : 'Usuń ofertę'),
              style: OutlinedButton.styleFrom(
                foregroundColor: Colors.redAccent,
                side: BorderSide(color: Colors.red.withValues(alpha: 0.4)),
                padding: const EdgeInsets.symmetric(vertical: 12),
              ),
            ),
          ),
          const SizedBox(height: 24),
        ],
      ),
    );
  }
}

/// Wraps a property form field with consistent top padding.
class _PropPad extends StatelessWidget {
  final Widget child;
  const _PropPad({required this.child});

  @override
  Widget build(BuildContext context) =>
      Padding(padding: const EdgeInsets.only(top: 12), child: child);
}

/// Square image preview with a remove (×) button in the top-right corner.
class _ImageThumb extends StatelessWidget {
  final Widget child;
  final VoidCallback onRemove;

  const _ImageThumb({required this.child, required this.onRemove});

  @override
  Widget build(BuildContext context) {
    return Stack(
      children: [
        ClipRRect(borderRadius: BorderRadius.circular(10), child: child),
        Positioned(
          right: 2,
          top: 2,
          child: GestureDetector(
            onTap: onRemove,
            child: Container(
              decoration: BoxDecoration(
                color: Colors.black.withValues(alpha: 0.65),
                shape: BoxShape.circle,
              ),
              padding: const EdgeInsets.all(2),
              child: const Icon(Icons.close, color: Colors.white, size: 14),
            ),
          ),
        ),
      ],
    );
  }
}
