class Seller {
  final int id;
  final String name;
  final double rating;

  const Seller({required this.id, required this.name, required this.rating});

  Seller.fromJson(Map<String, dynamic> json)
    : id = json['id'] ?? 0,
      name = json['name'] ?? '',
      rating = (json['rating'] ?? 0).toDouble();
}
