import 'package:frontend/Models/category.dart';
import 'package:frontend/Models/seller.dart';

class Offer {
  final int id;
  final String title;
  final String description;
  final double price;
  final List<String> images;
  final Seller seller;
  final Category category;
  final List<String> tags;
  final List<(String, String)> properties;
  final int availability;
  final String status;
  final DateTime createdAt;
  final DateTime updatedAt;

  const Offer({
    required this.id,
    required this.title,
    required this.description,
    required this.price,
    required this.images,
    required this.seller,
    required this.category,
    required this.tags,
    required this.properties,
    required this.availability,
    required this.status,
    required this.createdAt,
    required this.updatedAt,
  });

  Offer.fromJson(Map<String, dynamic> json)
    : id = json['id'] ?? 0,
      title = json['title'] ?? '',
      description = json['description'] ?? '',
      price = (json['price'] ?? 0).toDouble(),
      images = (json['images'] as List<dynamic>)
          .map((e) => e.toString())
          .toList(),
      seller = Seller.fromJson(json['seller']),
      category = Category.fromJson(json['category']),
      tags = (json['tags'] as List<dynamic>).map((e) => e.toString()).toList(),
      properties = (json['properties'] as Map<String, dynamic>).entries
          .map((e) => (e.key, e.value.toString()))
          .toList(),
      availability = json['availability'] ?? 0,
      status = json['status'] ?? 'ACTIVE',
      createdAt = DateTime.tryParse(json['createdAt'] ?? '') ?? DateTime.now(),
      updatedAt = DateTime.tryParse(json['updatedAt'] ?? '') ?? DateTime.now();
}
