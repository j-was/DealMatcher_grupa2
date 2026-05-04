class UserConversation {
  final int _id;
  final String _name;

  int get id => _id;
  String get name => _name;

  const UserConversation({int id = -1, required String name})
    : _id = id,
      _name = name;

  UserConversation.fromJson(Map<String, dynamic> json)
    : _id = json['id'] ?? 0,
      _name = json['name'] ?? '';

  Map<String, dynamic> toJson() => {'id': _id, 'name': _name};
}
