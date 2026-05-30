import 'package:frontend/Models/offer.dart';

class Conversation {
  final int _id;
  final int _offerId;
  final Offer _offer;
  final int _buyerId;
  final int _sellerId;
  final String _buyerName;
  final String _sellerName;
  final String _lastMessage;
  final DateTime _lastMessageAt;
  final int _unreadCount;
  final String _status;
  final DateTime _createdAt;

  int get id => _id;
  int get offerId => _offerId;
  Offer get offer => _offer;
  int get buyerId => _buyerId;
  int get sellerId => _sellerId;
  String get buyerName => _buyerName;
  String get sellerName => _sellerName;
  String get lastMessage => _lastMessage;
  DateTime get lastMessageAt => _lastMessageAt;
  int get unreadCount => _unreadCount;
  String get status => _status;
  DateTime get createdAt => _createdAt;

  Conversation.fromJson(Map<String, dynamic> json)
    : _id = json['id'] ?? 0,
      _offer = Offer.fromJson(json['offer'] as Map<String, dynamic>),
      _offerId = json['offer']?['id'] ?? json['offerId'] ?? 0,
      _buyerId = json['buyer']?['id'] ?? json['buyerId'] ?? 0,
      _sellerId = json['seller']?['id'] ?? json['sellerId'] ?? 0,
      _buyerName = json['buyer']?['name'] ?? '',
      _sellerName = json['seller']?['name'] ?? '',
      _lastMessage = json['lastMessage'] ?? '',
      _lastMessageAt =
          DateTime.tryParse(json['lastMessageAt'] ?? '') ?? DateTime.now(),
      _unreadCount = json['unreadCount'] ?? 0,
      _status = json['status'] ?? 'CLOSED',
      _createdAt = DateTime.tryParse(json['createdAt'] ?? '') ?? DateTime.now();
}
