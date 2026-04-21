class User {
  final int _id;
  final String _name;
  final String _surname;
  final String _email;
  final String _status;
  final DateTime _createdAt;

  int get id => _id;
  String get name => _name;
  String get surname => _surname;
  String get email => _email;
  String get status => _status;
  DateTime get createdAt => _createdAt;

  const User({
    int id = -1,
    required String name,
    required String surname,
    required String email,
    required String status,
    required DateTime createdAt,
  }) : _id = id,
       _name = name,
       _surname = surname,
       _email = email,
       _status = status,
       _createdAt = createdAt;
  User copyWith({
    int? id,
    String? name,
    String? surname,
    String? email,
    String? status,
    DateTime? createdAt,
  }) {
    return User(
      id: id ?? _id,
      name: name ?? _name,
      surname: surname ?? _surname,
      email: email ?? _email,
      status: status ?? _status,
      createdAt: createdAt ?? _createdAt,
    );
  }

  User.fromJson(Map json)
    : _id = json['id'] ?? 0,
      _name = json['name'] ?? '',
      _surname = json['surname'] ?? '',
      _email = json['email'] ?? '',
      _status = json['status'] ?? 'INACTIVE',
      _createdAt =
          DateTime.tryParse(json['createdAt']?.toString() ?? '') ??
          DateTime.now();
  Map<String, dynamic> toJson() => {
    'id': _id,
    'name': _name,
    'surname': _surname,
    'email': _email,
    'status': _status,
    'createdAt': _createdAt.toIso8601String(),
  };
}
