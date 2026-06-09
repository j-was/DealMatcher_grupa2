import 'dart:typed_data';
import 'package:flutter/material.dart';
import 'package:frontend/Models/category.dart';
import 'package:frontend/Models/category_property.dart';
import 'package:frontend/Models/offer_create.dart';
import 'package:frontend/Presentation/Widgets/seperated_widget.dart';
import 'package:frontend/Services/offer_service.dart';
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
  final OfferService _offerService = OfferService();

  List<Category> categories = [];
  List<CategoryProperty> categoryProperties = [];
  final Map<String, dynamic> _propertyValues = {};
  Category? _selectedCategory;

  final List<XFile> _images = [];
  final List<Uint8List> _imagesBytes = [];
  final ImagePicker _picker = ImagePicker();

  final _titleController = TextEditingController();
  final _descriptionController = TextEditingController();
  final _priceController = TextEditingController();
  final List<TextEditingController> _tagsControllers = [
    TextEditingController(),
  ];
  final _availabilityController = TextEditingController();

  final _formKey = GlobalKey<FormState>();

  Future<void> _pickImages() async {
    try {
      final List<XFile> picked = await _picker.pickMultiImage();

      if (picked.isEmpty) return;

      final remaining = 5 - _images.length;
      final selected = picked.take(remaining);

      final selectedImages = <XFile>[];
      final selectedImagesBytes = <Uint8List>[];

      for (final img in selected) {
        final bytes = await img.readAsBytes();
        selectedImages.add(img);
        selectedImagesBytes.add(bytes);
      }

      if (selectedImages.isEmpty) return;

      setState(() {
        _images.addAll(selectedImages);
        _imagesBytes.addAll(selectedImagesBytes);
      });
    } catch (e) {
      debugPrint("Błąd podczas wybierania zdjęć: $e");
    }
  }

  Future<void> getCategories() async {
    try {
      final gotCategories = await _categoryService.getCategories();
      setState(() {
        categories = gotCategories;
      });
    } catch (e) {
      debugPrint("Błąd ładowania kategorii: $e");
    }
  }

  Future<void> getProperties(Category category) async {
    setState(() {
      categoryProperties = [];
      _propertyValues.clear();
    });

    try {
      final properties = await _categoryService.getCategoryProperties(
        category.name,
      );

      setState(() {
        categoryProperties = properties;

        for (final property in properties) {
          final key = property.id.toString();
          switch (property.type) {
            case "BOOLEAN":
              _propertyValues[key] = false;
              break;
            case "SELECT":
              _propertyValues[key] = property.options.isNotEmpty
                  ? property.options.first
                  : null;
              break;
            default:
              _propertyValues[key] = '';
              break;
          }
        }
      });
    } catch (e) {
      debugPrint('Błąd ładowania właściwości kategorii: $e');
    }
  }

  Widget buildPropertyField(CategoryProperty property) {
    final key = property.id.toString();
    switch (property.type) {
      case "TEXT":
        return SeparatedWidget(
          widget: TextFormField(
            initialValue: (_propertyValues[key] ?? '').toString(),
            decoration: InputDecoration(labelText: property.name),
            style: const TextStyle(color: Colors.white54),
            autovalidateMode: AutovalidateMode.onUserInteraction,
            onChanged: (value) {
              _propertyValues[key] = value;
            },
            validator: (value) {
              if (value == null || value.trim().isEmpty) {
                return 'Wymagana jest wartość właściwości';
              }
              return null;
            },
          ),
        );
      case "NUMBER":
        return SeparatedWidget(
          widget: TextFormField(
            initialValue: (_propertyValues[key] ?? '').toString(),
            decoration: InputDecoration(labelText: property.name),
            style: const TextStyle(color: Colors.white54),
            autovalidateMode: AutovalidateMode.onUserInteraction,
            onChanged: (value) {
              _propertyValues[key] = value;
            },
            validator: (value) {
              if (value == null || value.trim().isEmpty) {
                return 'Wymagana jest wartość właściwości';
              }
              if (num.tryParse(value) == null) {
                return 'Wartość musi być liczbą';
              }
              return null;
            },
          ),
        );
      case "BOOLEAN":
        return SeparatedWidget(
          widget: SwitchListTile(
            title: Text(
              property.name,
              style: const TextStyle(color: Colors.white54),
            ),
            value: (_propertyValues[key] as bool?) ?? false,
            onChanged: (value) {
              setState(() {
                _propertyValues[key] = value;
              });
            },
          ),
        );
      case "SELECT":
        return SeparatedWidget(
          widget: DropdownButtonFormField<String>(
            initialValue: _propertyValues[key] as String?,
            decoration: InputDecoration(labelText: property.name),
            dropdownColor: const Color.fromARGB(255, 64, 63, 63),
            style: const TextStyle(color: Colors.white54),
            items: property.options
                .map(
                  (option) => DropdownMenuItem<String>(
                    value: option,
                    child: Text(option),
                  ),
                )
                .toList(),
            onChanged: (value) {
              setState(() {
                _propertyValues[key] = value;
              });
            },
            validator: (value) {
              if (value == null || value.isEmpty) {
                return 'Wybierz opcję';
              }
              return null;
            },
          ),
        );
      default:
        return SeparatedWidget(
          widget: Text(
            'Nieobsługiwany typ pola: ${property.type}',
            style: const TextStyle(color: Colors.redAccent),
          ),
        );
    }
  }

  List<Widget> generateAllPropertiesControls() {
    final List<Widget> controls = [];
    for (var property in categoryProperties) {
      controls.add(buildPropertyField(property));
    }
    return controls;
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
                    ...List.generate(
                      _imagesBytes.length,
                      (index) => Stack(
                        children: [
                          Image.memory(
                            _imagesBytes[index],
                            width: 80,
                            height: 80,
                            fit: BoxFit.cover,
                          ),
                          Positioned(
                            right: 0,
                            top: 0,
                            child: GestureDetector(
                              onTap: () {
                                setState(() {
                                  _imagesBytes.removeAt(index);
                                  _images.removeAt(index);
                                });
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
            widget: DropdownButtonFormField<Category>(
              initialValue: _selectedCategory,
              decoration: const InputDecoration(labelText: 'Kategoria oferty'),
              style: TextStyle(color: Colors.white54),
              dropdownColor: const Color.fromARGB(255, 64, 63, 63),
              items: categories.map((category) {
                return DropdownMenuItem<Category>(
                  value: category,
                  child: Text(category.name),
                );
              }).toList(),
              onChanged: (Category? value) async {
                setState(() {
                  _selectedCategory = value;
                });

                await getProperties(_selectedCategory!);
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
          ...generateAllPropertiesControls(),
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
                key: const Key('addOfferSubmitButton'),
                onPressed: () async {
                  if (!(_formKey.currentState?.validate() ?? false)) {
                    return;
                  }

                  try {
                    final tags = _tagsControllers.map((c) => c.text).toList();
                    final properties = <String, String>{
                      for (final property in categoryProperties)
                        property.id.toString():
                            _propertyValues[property.id.toString()].toString(),
                    };

                    final offer = OfferCreate(
                      title: _titleController.text,
                      description: _descriptionController.text,
                      price: double.parse(_priceController.text),
                      categoryId: _selectedCategory?.id ?? 1,
                      tags: tags,
                      properties: properties,
                      availability: int.parse(_availabilityController.text),
                    );

                    await _offerService.createOffer(offer.toJson(), _images);

                    if (!context.mounted) return;

                    context.go('/');
                  } catch (e) {
                    if (!context.mounted) return;

                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(content: Text('Nie udało się dodać oferty')),
                    );
                  }
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
