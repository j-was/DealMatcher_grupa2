import 'dart:typed_data';

import 'package:flutter/material.dart';
import 'package:frontend/Models/category.dart';
import 'package:frontend/Presentation/Widgets/seperated_widget.dart';
import 'package:frontend/Services/category_service.dart';
import 'package:go_router/go_router.dart';
import 'package:image_picker/image_picker.dart';

class AddOfferForm extends StatefulWidget {
  const AddOfferForm({super.key});

  @override
  State<AddOfferForm> createState() => AddOfferFormState();
}

class AddOfferFormState extends State<AddOfferForm> {
  final _categoryService = CategoryService();
  List<Category> _categories = [];
  Category? _selectedCategory;
  bool _isLoadingCategories = true;

  final List<Uint8List> _images = [];
  final ImagePicker _picker = ImagePicker();

  final _titleController = TextEditingController();
  final _descriptionController = TextEditingController();
  final _priceController = TextEditingController();
  final List<TextEditingController> _tagsControllers = [
    TextEditingController(),
  ];

  final List<(TextEditingController, TextEditingController)>
  _propertiesControllers = [(TextEditingController(), TextEditingController())];
  final _availabilityController = TextEditingController();

  final _formKey = GlobalKey<FormState>();

  Future<void> _pickImages() async {
    try {
      final List<XFile> picked = await _picker.pickMultiImage();

      if (picked.isEmpty) return;

      final remaining = 5 - _images.length;
      final selected = picked.take(remaining);

      for (final img in selected) {
        final bytes = await img.readAsBytes();
        _images.add(bytes);
      }

      setState(() {});
    } catch (e) {
      debugPrint("Błąd podczas wybierania zdjęć: $e");
    }
  }

  Future<void> getCategories() async {
    try {
      final categories = await _categoryService.getCategories();
      setState(() {
        _categories = categories;
        _isLoadingCategories = false;
      });
    } catch (e) {
      debugPrint("Błąd ładowania kategorii: $e");
      setState(() {
        _isLoadingCategories = false;
      });
    }
  }

  void _addTag() {
    if (_tagsControllers.length >= 10) {
      return;
    }

    setState(() {
      _tagsControllers.add(TextEditingController());
    });
  }

  void _removeTag() {
    if (_tagsControllers.length == 1) {
      return;
    }

    setState(() {
      _tagsControllers.removeLast();
    });
  }

  void _addProperty() {
    if (_propertiesControllers.length >= 10) {
      return;
    }

    setState(() {
      _propertiesControllers.add((
        TextEditingController(),
        TextEditingController(),
      ));
    });
  }

  void _removeProperty() {
    if (_propertiesControllers.length == 1) {
      return;
    }

    setState(() {
      _propertiesControllers.removeLast();
    });
  }

  @override
  void initState() {
    getCategories();
    super.initState();
  }

  @override
  void dispose() {
    _titleController.dispose();
    _descriptionController.dispose();
    _priceController.dispose();
    _availabilityController.dispose();
    for (var ctrl in _tagsControllers) {
      ctrl.dispose();
    }

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Form(
      key: _formKey,
      child: ListView(
        children: [
          SizedBox(height: 12),
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('titleField'),
              controller: _titleController,
              decoration: InputDecoration(labelText: 'Tytuł oferty'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "Wymagany tytuł oferty";
                }
                return null;
              },
              style: TextStyle(color: Colors.white54),
            ),
          ),
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('descriptionField'),
              controller: _descriptionController,
              decoration: InputDecoration(labelText: 'Opis'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              keyboardType: TextInputType.multiline,
              maxLines: null,
              minLines: 3,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "Wymagany opis";
                }
                return null;
              },
              style: TextStyle(color: Colors.white54),
            ),
          ),
          Padding(
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const Text(
                  'Dodaj obrazy',
                  style: TextStyle(fontSize: 16, color: Colors.white54),
                ),
                const SizedBox(height: 8),

                Wrap(
                  spacing: 8,
                  runSpacing: 8,
                  children: [
                    ..._images.map(
                      (img) => Stack(
                        children: [
                          Image.memory(
                            img,
                            width: 80,
                            height: 80,
                            fit: BoxFit.cover,
                          ),
                          Positioned(
                            right: 0,
                            top: 0,
                            child: GestureDetector(
                              onTap: () {
                                setState(() => _images.remove(img));
                              },
                              child: const Icon(Icons.close, color: Colors.red),
                            ),
                          ),
                        ],
                      ),
                    ),
                    if (_images.length < 5)
                      GestureDetector(
                        onTap: _pickImages,
                        child: Container(
                          width: 80,
                          height: 80,
                          color: Colors.grey[800],
                          child: const Icon(Icons.add),
                        ),
                      ),
                  ],
                ),
              ],
            ),
          ),
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('priceField'),
              controller: _priceController,
              decoration: InputDecoration(labelText: 'Cena'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              keyboardType: TextInputType.number,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "Wymagana cena oferty";
                }
                final number = int.tryParse(value);
                if (number == null) {
                  return "Podaj poprawną liczbę";
                }
                return null;
              },
              style: TextStyle(color: Colors.white54),
            ),
          ),
          SeparatedWidget(
            widget: _isLoadingCategories
                ? const Center(child: CircularProgressIndicator())
                : DropdownButtonFormField<Category>(
                    initialValue: _selectedCategory,
                    decoration: const InputDecoration(
                      labelText: 'Kategoria oferty',
                    ),
                    items: _categories.map((category) {
                      return DropdownMenuItem<Category>(
                        value: category,
                        child: Text(category.name),
                      );
                    }).toList(),
                    onChanged: (Category? value) {
                      setState(() {
                        _selectedCategory = value;
                      });
                    },
                    validator: (value) {
                      if (value == null) {
                        return 'Wybierz kategorię';
                      }
                      return null;
                    },
                  ),
          ),
          ...List.generate(
            _tagsControllers.length,
            (i) => SeparatedWidget(
              widget: TextFormField(
                key: Key('tagField$i'),
                controller: _tagsControllers[i],
                decoration: InputDecoration(labelText: 'Tag ${i + 1}'),
                autovalidateMode: AutovalidateMode.onUserInteraction,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return "Wymagany tag oferty";
                  }
                  return null;
                },
                style: TextStyle(color: Colors.white54),
              ),
            ),
          ),
          if (_tagsControllers.length < 5 || _tagsControllers.length > 1)
            SeparatedWidget(
              widget: Center(
                child: SizedBox(
                  width: 200,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceAround,
                    children: [
                      if (_tagsControllers.length > 1)
                        IconButton(
                          onPressed: () {
                            _removeTag();
                          },
                          icon: Icon(Icons.remove),
                        ),
                      if (_tagsControllers.length < 10)
                        IconButton(
                          onPressed: () {
                            _addTag();
                          },
                          icon: Icon(Icons.add),
                        ),
                    ],
                  ),
                ),
              ),
            ),

          ...List.generate(
            _propertiesControllers.length,
            (i) => SeparatedWidget(
              widget: Row(
                children: [
                  Expanded(
                    flex: 5,
                    child: TextFormField(
                      key: Key('propertyTitleField$i'),
                      controller: _propertiesControllers[i].$1,
                      decoration: InputDecoration(
                        labelText: 'Nazwa cechy ${i + 1}',
                      ),
                      autovalidateMode: AutovalidateMode.onUserInteraction,
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return "Wymagana nazwa cechy oferty";
                        }
                        return null;
                      },
                      style: TextStyle(color: Colors.white54),
                    ),
                  ),
                  SizedBox(width: 12),
                  Expanded(
                    flex: 5,
                    child: TextFormField(
                      key: Key('propertyValueField$i'),
                      controller: _propertiesControllers[i].$2,
                      decoration: InputDecoration(
                        labelText: 'Wartość cechy ${i + 1}',
                      ),
                      autovalidateMode: AutovalidateMode.onUserInteraction,
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return "Wymagana wartość cechy oferty";
                        }
                        return null;
                      },
                      style: TextStyle(color: Colors.white54),
                    ),
                  ),
                ],
              ),
            ),
          ),
          if (_propertiesControllers.length < 5 ||
              _propertiesControllers.length > 1)
            SeparatedWidget(
              widget: Center(
                child: SizedBox(
                  width: 200,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceAround,
                    children: [
                      if (_propertiesControllers.length > 1)
                        IconButton(
                          onPressed: () {
                            _removeProperty();
                          },
                          icon: Icon(Icons.remove),
                        ),
                      if (_propertiesControllers.length < 10)
                        IconButton(
                          onPressed: () {
                            _addProperty();
                          },
                          icon: Icon(Icons.add),
                        ),
                    ],
                  ),
                ),
              ),
            ),
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('availabilityField'),
              controller: _availabilityController,
              decoration: InputDecoration(labelText: 'Ilość ofert'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              keyboardType: TextInputType.number,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "Wymagana ilość ofert";
                }
                final number = int.tryParse(value);
                if (number == null) {
                  return "Podaj poprawną liczbę";
                }
                return null;
              },
              style: TextStyle(color: Colors.white54),
            ),
          ),
          Center(
            child: SizedBox(
              width: 120,
              child: ElevatedButton(
                key: const Key('signUpSubmitButton'),
                onPressed: () async {
                  if (!(_formKey.currentState?.validate() ?? false)) {
                    return;
                  }
                  context.pop();
                  context.go('/');
                },
                child: Text(
                  'Wyślij',
                  style: TextStyle(fontSize: 18, color: Colors.white),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
