class Seller {
  final int _id;
  final String _name;
  final double _rating;

  int get id => _id;
  String get name => _name;
  double get rating => _rating;

  const Seller({int id = -1, required String name, required double rating})
    : _id = id,
      _name = name,
      _rating = rating;

  Seller.fromJson(Map<String, dynamic> json)
    : _id = json['id'] ?? 0,
      _name = json['name'] ?? '',
      _rating = (json['rating'] ?? 0).toDouble();

  Map<String, dynamic> toJson() => {
    'id': _id,
    'name': _name,
    'rating': _rating,
  };
}
