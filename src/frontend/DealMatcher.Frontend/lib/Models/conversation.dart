class Conversation {
  final int _id;
  final int _offerId;
  final int _buyerId;
  final int _sellerId;
  final String _lastMessage;
  final DateTime _lastMessageAt;
  final int _unreadCount;
  final String _status;
  final DateTime _createdAt;

  int get id => _id;
  int get offerId => _offerId;
  int get buyerId => _buyerId;
  int get sellerId => _sellerId;
  String get lastMessage => _lastMessage;
  DateTime get lastMessageAt => _lastMessageAt;
  int get unreadCount => _unreadCount;
  String get status => _status;
  DateTime get createdAt => _createdAt;

  Conversation.fromJson(Map<String, dynamic> json)
    : _id = json['id'] ?? 0,
      _offerId = json['offerId'] ?? 0,
      _buyerId = json['buyerId'] ?? 0,
      _sellerId = json['sellerId'] ?? 0,
      _lastMessage = json['lastMessage'] ?? '',
      _lastMessageAt =
          DateTime.tryParse(json['lastMessageAt'] ?? '') ?? DateTime.now(),
      _unreadCount = json['unreadCount'] ?? 0,
      _status = json['status'] ?? 'CLOSED',
      _createdAt = DateTime.tryParse(json['createdAt'] ?? '') ?? DateTime.now();
}
