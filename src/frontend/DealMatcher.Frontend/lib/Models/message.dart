class Message {
  final int _id;
  final int _senderId;
  final String _content;
  final String _status;
  final DateTime _createdAt;

  int get id => _id;
  int get senderId => _senderId;
  String get content => _content;
  String get status => _status;
  DateTime get createdAt => _createdAt;

  const Message({
    int id = -1,
    required int senderId,
    required String content,
    required String status,
    required DateTime createdAt,
  }) : _id = id,
       _senderId = senderId,
       _content = content,
       _status = status,
       _createdAt = createdAt;

  Message.fromJson(Map<String, dynamic> json)
    : _id = json['id'] ?? -1,
      _senderId = json['senderId'] ?? -1,
      _content = json['content'] ?? '',
      _status = json['status'] ?? '',
      _createdAt = DateTime.tryParse(json['createdAt']) ?? DateTime.now();

  Map<String, dynamic> toJson() => {
    'id': _id,
    'senderId': _senderId,
    'content': _content,
    'status': _status,
    'createdAt': _createdAt.toIso8601String(),
  };
}
