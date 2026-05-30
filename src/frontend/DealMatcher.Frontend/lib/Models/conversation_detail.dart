import 'package:frontend/Models/message.dart';

class ConversationDetail {
  final int _id;
  final int _offerId;
  final int _buyerId;
  final int _sellerId;
  final String _buyerName;
  final String _sellerName;
  final String _lastMessage;
  final DateTime _lastMessageAt;
  final int _unreadCount;
  final String _status;
  final DateTime _createdAt;
  final List<Message> _messages;

  int get id => _id;
  int get offerId => _offerId;
  int get buyerId => _buyerId;
  int get sellerId => _sellerId;
  String get buyerName => _buyerName;
  String get sellerName => _sellerName;
  String get lastMessage => _lastMessage;
  DateTime get lastMessageAt => _lastMessageAt;
  int get unreadCount => _unreadCount;
  String get status => _status;
  DateTime get createdAt => _createdAt;
  List<Message> get messages => _messages;

  const ConversationDetail({
    int id = -1,
    required int offerId,
    required int buyerId,
    required int sellerId,
    String buyerName = '',
    String sellerName = '',
    required String lastMessage,
    required DateTime lastMessageAt,
    int unreadCount = 0,
    String status = '',
    required DateTime createdAt,
    required List<Message> messages,
  }) : _id = id,
       _offerId = offerId,
       _buyerId = buyerId,
       _sellerId = sellerId,
       _buyerName = buyerName,
       _sellerName = sellerName,
       _lastMessage = lastMessage,
       _lastMessageAt = lastMessageAt,
       _unreadCount = unreadCount,
       _status = status,
       _createdAt = createdAt,
       _messages = messages;

  ConversationDetail.fromJson(Map<String, dynamic> json)
    : _id = json['id'] ?? 0,
      _offerId = json['offerId'] ?? json['offer']?['id'] ?? 0,
      _buyerId = json['buyer']?['id'] ?? json['buyerId'] ?? 0,
      _sellerId = json['seller']?['id'] ?? json['sellerId'] ?? 0,
      _buyerName = json['buyer']?['name'] ?? '',
      _sellerName = json['seller']?['name'] ?? '',
      _lastMessage = json['lastMessage'] ?? '',
      _lastMessageAt =
          DateTime.tryParse(json['lastMessageAt'] ?? '') ?? DateTime.now(),
      _unreadCount = json['unreadCount'] ?? 0,
      _status = json['status'] ?? 'CLOSED',
      _createdAt =
          DateTime.tryParse(json['createdAt']?.toString() ?? '') ??
          DateTime.now(),
      _messages = (json['messages'] as List<dynamic>)
          .map((e) => Message.fromJson(e as Map<String, dynamic>))
          .toList();

  Map<String, dynamic> toJson() => {
    'id': _id,
    'offerId': _offerId,
    'buyer': {'id': _buyerId, 'name': _buyerName},
    'seller': {'id': _sellerId, 'name': _sellerName},
    'lastMessage': _lastMessage,
    'lastMessageAt': _lastMessageAt.toIso8601String(),
    'unreadCount': _unreadCount,
    'status': _status,
    'createdAt': _createdAt.toIso8601String(),
    'messages': _messages.map((e) => e.toJson()).toList(),
  };
}
