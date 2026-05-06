class Category {
  final int _id;
  final String _name;
  final String _description;

  int get id => _id;
  String get name => _name;
  String get description => _description;

  const Category({
    int id = -1,
    required String name,
    required String description,
  }) : _id = id,
       _name = name,
       _description = description;

  Category.fromJson(Map<String, dynamic> json)
    : _id = json['id'] ?? 0,
      _name = json['name'] ?? '',
      _description = json['description'] ?? '';

  Map<String, dynamic> toJson() => {
    'id': _id,
    'name': _name,
    'description': _description,
  };
}
