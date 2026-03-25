class Category {
  final int id;
  final String name;
  final String description;

  const Category({
    required this.id,
    required this.name,
    required this.description,
  });

  Category.fromJson(Map<String, dynamic> json)
    : id = json['id'] ?? 0,
      name = json['name'] ?? '',
      description = json['description'] ?? '';
}
