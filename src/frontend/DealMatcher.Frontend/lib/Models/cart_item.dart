import 'package:frontend/Models/offer.dart';

class CartItem {
  final int _id;
  final Offer _offer;
  final int _quantity;
  final DateTime _addedAt;

  int get id => _id;
  Offer get offer => _offer;
  int get quantity => _quantity;
  DateTime get addedAt => _addedAt;

  const CartItem({
    required int id,
    required Offer offer,
    required int quantity,
    required DateTime addedAt,
  }) : _id = id,
       _offer = offer,
       _quantity = quantity,
       _addedAt = addedAt;
}
