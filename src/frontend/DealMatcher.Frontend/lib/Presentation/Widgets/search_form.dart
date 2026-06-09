import 'package:flutter/material.dart';
import 'package:frontend/Models/category.dart';
import 'package:frontend/Models/category_property.dart';
import 'package:frontend/Models/search_params.dart';
import 'package:frontend/Presentation/Widgets/seperated_widget.dart';
import 'package:frontend/Services/offer_service.dart';
import 'package:frontend/Services/category_service.dart';
import 'package:go_router/go_router.dart';

class SearchForm extends StatefulWidget {
  const SearchForm({super.key});

  @override
  State<SearchForm> createState() => SearchFormState();
}

class SearchFormState extends State<SearchForm> {
  final _categoryService = CategoryService();
  final OfferService _offerService = OfferService();

  final List<int> _limitChoices = [10, 20, 50, 100, 200];
  int _selectedLimit = 10;

  List<Category> categories = [];
  List<CategoryProperty> categoryProperties = [];
  final Map<String, List<dynamic>> _propertyValues = {};
  Category? _selectedCategory;

  final Map<String, List<TextEditingController>> _textPropertiesControllers =
      {};
  final _searchPhraseController = TextEditingController();
  final _limitController = TextEditingController();
  final _minPriceController = TextEditingController();
  final _maxPriceController = TextEditingController();
  final List<TextEditingController> _tagsControllers = [
    TextEditingController(),
  ];

  final _formKey = GlobalKey<FormState>();

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
            case "TEXT":
              _textPropertiesControllers[key] = [TextEditingController()];
              _propertyValues[key] = [''];
              break;
            case "NUMBER":
              _propertyValues[key] = ['', ''];
              break;
            case "BOOLEAN":
              _propertyValues[key] = [false];
              break;
            case "SELECT":
              _propertyValues[key] = [
                property.options.isNotEmpty ? property.options.first : null,
              ];
              break;
            default:
              _propertyValues[key] = [''];
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
        final controllers = _textPropertiesControllers[key] ??= [
          TextEditingController(),
        ];
        return SeparatedWidget(
          widget: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              ...List.generate(controllers.length, (i) {
                return Padding(
                  padding: const EdgeInsets.only(bottom: 6),
                  child: TextFormField(
                    controller: controllers[i],
                    decoration: InputDecoration(
                      labelText: '${property.name} ${i + 1}',
                    ),
                    style: const TextStyle(color: Colors.white54),
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    onChanged: (_) {
                      _propertyValues[key] = controllers
                          .map((ctrl) => ctrl.text)
                          .toList();
                    },
                    validator: (value) {
                      if (value == null || value.trim().isEmpty) {
                        return 'Wymagana jest wartość właściwości';
                      }
                      return null;
                    },
                  ),
                );
              }),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  if (controllers.isNotEmpty)
                    IconButton(
                      onPressed: () {
                        removeTextProperty(key);
                      },
                      icon: const Icon(Icons.remove),
                    ),
                  IconButton(
                    onPressed: () {
                      addTextProperty(key);
                    },
                    icon: const Icon(Icons.add),
                  ),
                ],
              ),
            ],
          ),
        );
      case "NUMBER":
        final values = (_propertyValues[key]) ?? ['', ''];
        return SeparatedWidget(
          widget: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SeparatedWidget(
                widget: Text(
                  property.name,
                  style: const TextStyle(color: Colors.white54),
                ),
              ),
              SeparatedWidget(
                widget: Row(
                  children: [
                    Expanded(
                      child: TextFormField(
                        initialValue: values[0]?.toString() ?? '',
                        decoration: InputDecoration(
                          labelText: '${property.name} min',
                        ),
                        style: const TextStyle(color: Colors.white54),
                        autovalidateMode: AutovalidateMode.onUserInteraction,
                        onChanged: (value) {
                          _propertyValues[key] = [value, values[1]];
                        },
                        validator: (value) {
                          if (value != null &&
                              value.trim().isNotEmpty &&
                              num.tryParse(value) == null) {
                            return 'Minimalna wartość właściwości musi być liczbą';
                          }
                          return null;
                        },
                      ),
                    ),
                    SizedBox(width: 40),
                    Expanded(
                      child: TextFormField(
                        initialValue: values[1]?.toString() ?? '',
                        decoration: InputDecoration(
                          labelText: '${property.name} max',
                        ),
                        style: const TextStyle(color: Colors.white54),
                        autovalidateMode: AutovalidateMode.onUserInteraction,
                        onChanged: (value) {
                          _propertyValues[key] = [values[0], value];
                        },
                        validator: (value) {
                          if (value != null &&
                              value.trim().isNotEmpty &&
                              num.tryParse(value) == null) {
                            return 'Maksymalna wartość właściwości musi być liczbą';
                          }
                          final minValue = num.tryParse(
                            (values[0] ?? '').toString(),
                          );
                          final maxValue = num.tryParse((value ?? '').trim());

                          if (minValue != null &&
                              maxValue != null &&
                              minValue >= maxValue) {
                            return 'Maksymalna wartość właściwości nie może być mniejsza od minimalnej';
                          }
                          return null;
                        },
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        );
      case "BOOLEAN":
        return SeparatedWidget(
          widget: SwitchListTile(
            title: Text(
              property.name,
              style: const TextStyle(color: Colors.white54),
            ),
            value: (_propertyValues[key]?.first as bool?) ?? false,
            onChanged: (value) {
              setState(() {
                _propertyValues[key] = [value];
              });
            },
          ),
        );
      case "SELECT":
        final selectedValues = (_propertyValues[key] ?? [])
            .whereType<String>()
            .toList();
        return SeparatedWidget(
          widget: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Padding(
                padding: const EdgeInsets.only(bottom: 6),
                child: Text(
                  property.name,
                  style: const TextStyle(color: Colors.white54),
                ),
              ),
              ...property.options.map((o) {
                final isSelected = selectedValues.contains(o);

                return CheckboxListTile(
                  title: Text(o, style: const TextStyle(color: Colors.white54)),
                  value: isSelected,
                  controlAffinity: ListTileControlAffinity.leading,
                  onChanged: (checked) {
                    setState(() {
                      final currValues = (_propertyValues[key] ?? [])
                          .whereType<String>()
                          .toList();

                      if (checked == true) {
                        if (!currValues.contains(o)) {
                          currValues.add(o);
                        }
                      } else {
                        currValues.remove(o);
                      }

                      _propertyValues[key] = currValues;
                    });
                  },
                );
              }),
            ],
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
    if (_tagsControllers.isEmpty) {
      return;
    }

    setState(() {
      _tagsControllers.removeLast();
    });
  }

  void addTextProperty(String propertyName) {
    setState(() {
      _textPropertiesControllers[propertyName]?.add(TextEditingController());
    });
  }

  void removeTextProperty(String propertyName) {
    if (_textPropertiesControllers[propertyName]?.isEmpty ?? true) {
      return;
    }

    setState(() {
      _textPropertiesControllers[propertyName]?.removeLast();
    });
  }

  Map<String, dynamic> buildSearchParamsRequest() {
    final tags = _tagsControllers
        .map((c) => c.text.trim())
        .where((t) => t.isNotEmpty)
        .toList();
    final properties = <String, List<String>>{};

    for (final property in categoryProperties) {
      final key = property.id.toString();
      final values = _propertyValues[key];

      if (values == null) {
        continue;
      }

      final strValues = values
          .map((v) => v?.toString().trim() ?? '')
          .where((v) => v.isNotEmpty)
          .toList();

      if (strValues.isNotEmpty) {
        properties[key] = strValues;
      }
    }

    final searchParams = SearchParams(
      categoryId: _selectedCategory?.id,
      minPrice: double.tryParse(_minPriceController.text),
      maxPrice: double.tryParse(_maxPriceController.text),
      tags: tags,
      properties: properties,
      searchPhrase: _searchPhraseController.text,
      limit: _selectedLimit,
    );

    return searchParams.toJson();
  }

  @override
  void initState() {
    super.initState();
    _minPriceController.text = '0';
    _maxPriceController.text = '10000';
    _limitController.text = '10';
    getCategories();
  }

  @override
  void dispose() {
    for (var ctrl in _tagsControllers) {
      ctrl.dispose();
    }
    for (final controllers in _textPropertiesControllers.values) {
      for (final controller in controllers) {
        controller.dispose();
      }
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
              controller: _searchPhraseController,
              decoration: InputDecoration(
                labelText: 'Szukaj',
                prefixIcon: const Icon(Icons.search, color: Colors.white38),
                filled: true,
                fillColor: Colors.grey[800],
              ),
              style: const TextStyle(color: Colors.white54),
              autovalidateMode: AutovalidateMode.onUserInteraction,
            ),
          ),
          SeparatedWidget(
            widget: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Expanded(
                  child: TextFormField(
                    controller: _minPriceController,
                    decoration: InputDecoration(labelText: 'Min. cena'),
                    style: const TextStyle(color: Colors.white54),
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    validator: (value) {
                      if (value != null &&
                          value.trim().isNotEmpty &&
                          num.tryParse(value) == null) {
                        return 'Minimalna cena musi być liczbą';
                      }
                      return null;
                    },
                  ),
                ),
                SizedBox(width: 40),
                Expanded(
                  child: TextFormField(
                    controller: _maxPriceController,
                    decoration: InputDecoration(labelText: 'Max. cena'),
                    style: const TextStyle(color: Colors.white54),
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    validator: (value) {
                      if (value != null &&
                          value.trim().isNotEmpty &&
                          num.tryParse(value) == null) {
                        return 'Maksymalna cena musi być liczbą';
                      }
                      final minValue = num.tryParse(
                        (_minPriceController.text).toString(),
                      );
                      final maxValue = num.tryParse((value ?? '').trim());

                      if (minValue != null &&
                          maxValue != null &&
                          minValue >= maxValue) {
                        return 'Maksymalna cena nie może być mniejsza od minimalnej';
                      }
                      return null;
                    },
                  ),
                ),
              ],
            ),
          ),
          SeparatedWidget(
            widget: DropdownButtonFormField<int>(
              initialValue: _selectedLimit,
              decoration: const InputDecoration(
                labelText: 'Maksymalna ilość szukanych ofert',
              ),
              style: TextStyle(color: Colors.white54),
              dropdownColor: const Color.fromARGB(255, 64, 63, 63),
              items: _limitChoices.map((limit) {
                return DropdownMenuItem<int>(
                  value: limit,
                  child: Text(limit.toString()),
                );
              }).toList(),
              onChanged: (int? value) async {
                setState(() {
                  _selectedLimit = value ?? _selectedLimit;
                });
              },
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
                      if (_tagsControllers.isNotEmpty)
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
          Center(
            child: SizedBox(
              width: 120,
              child: ElevatedButton(
                key: const Key('searchSubmitButton'),
                onPressed: () async {
                  if (!_formKey.currentState!.validate()) {
                    return;
                  }

                  try {
                    final searchParamsRequest = buildSearchParamsRequest();
                    final offers = await _offerService.searchOffers(
                      searchParamsRequest,
                    );

                    if (!context.mounted) {
                      return;
                    }

                    context.push('/searched', extra: offers);
                  } catch (e) {
                    if (!context.mounted) {
                      return;
                    }

                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(content: Text('Błąd wyszukiwania ofert: $e')),
                    );
                  }
                },
                child: Text(
                  'Szukaj',
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
