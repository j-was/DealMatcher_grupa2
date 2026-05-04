class DeliveryMethod {
  final String id;
  final String name;
  final String description;
  final double price;
  final int estimatedDays;

  const DeliveryMethod({
    required this.id,
    required this.name,
    required this.description,
    required this.price,
    required this.estimatedDays,
  });

  factory DeliveryMethod.fromJson(Map<String, dynamic> json) {
    return DeliveryMethod(
      id: json['id']?.toString() ?? '',
      name: json['name']?.toString() ?? '',
      description: json['description']?.toString() ?? '',
      price: (json['price'] ?? 0).toDouble(),
      estimatedDays: (json['estimatedDays'] ?? 0) as int,
    );
  }
}
