class CategoryProperty {
  final int _id;
  final String _name;
  final int _type;
  final List<String> _options;

  int get id => _id;
  String get name => _name;
  String get type => _type;
  List<String> get options => _options;

  const CategoryProperty({
    int id = -1,
    required String name,
    required String description,
    required int type,
    required List<String> options,
  }) : _id = id,
       _name = name,
       _type = type,
       _options = options;

  CategoryProperty.fromJson(Map<String, dynamic> json)
    : _id = json['id'] ?? 0,
      _name = json['name'] ?? '',
      _type = json['type'] ?? 1,
      _options = List<String>.from(json['options'] ?? []);
}
