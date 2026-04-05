import 'package:frontend/Models/category.dart';
import 'package:frontend/Models/seller.dart';

class Offer {
  final int _id;
  final String _title;
  final String _description;
  final double _price;
  final List<String> _images;
  final Seller _seller;
  final Category _category;
  final List<String> _tags;
  final List<(String, String)> _properties;
  final int _availability;
  final String _status;
  final DateTime _createdAt;
  final DateTime _updatedAt;

  int get id => _id;
  String get title => _title;
  String get description => _description;
  double get price => _price;
  List<String> get images => List.unmodifiable(_images);
  Seller get seller => _seller;
  Category get category => _category;
  List<String> get tags => List.unmodifiable(_tags);
  List<(String, String)> get properties => List.unmodifiable(_properties);
  int get availability => _availability;
  String get status => _status;
  DateTime get createdAt => _createdAt;
  DateTime get updatedAt => _updatedAt;

  const Offer({
    int id = -1,
    required String title,
    required String description,
    required double price,
    required List<String> images,
    required Seller seller,
    required Category category,
    required List<String> tags,
    required List<(String, String)> properties,
    required int availability,
    required String status,
    required DateTime createdAt,
    required DateTime updatedAt,
  }) : _id = id,
       _title = title,
       _description = description,
       _price = price,
       _images = images,
       _seller = seller,
       _category = category,
       _tags = tags,
       _properties = properties,
       _availability = availability,
       _status = status,
       _createdAt = createdAt,
       _updatedAt = updatedAt;

  Offer.fromJson(Map<String, dynamic> json)
    : _id = json['id'] ?? 0,
      _title = json['title'] ?? '',
      _description = json['description'] ?? '',
      _price = (json['price'] ?? 0).toDouble(),
      _images = (json['images'] as List<dynamic>)
          .map((e) => e.toString())
          .toList(),
      _seller = Seller.fromJson(json['seller']),
      _category = Category.fromJson(json['category']),
      _tags = (json['tags'] as List<dynamic>).map((e) => e.toString()).toList(),
      _properties = (json['properties'] as Map<String, dynamic>).entries
          .map((e) => (e.key, e.value.toString()))
          .toList(),
      _availability = json['availability'] ?? 0,
      _status = json['status'] ?? 'ACTIVE',
      _createdAt = DateTime.tryParse(json['createdAt'] ?? '') ?? DateTime.now(),
      _updatedAt = DateTime.tryParse(json['updatedAt'] ?? '') ?? DateTime.now();
}
