class ActivityRecord {
  final int _id;
  final int _userId;
  final int? _offerId;
  final String _action;
  final Map<String, dynamic> _details;
  final String _ipAddress;
  final DateTime _createdAt;

  int get id => _id;
  int get userId => _userId;
  int? get offerId => _offerId;
  String get action => _action;
  Map<String, dynamic> get details => Map.unmodifiable(_details);
  String get ipAddress => _ipAddress;
  DateTime get createdAt => _createdAt;

  const ActivityRecord({
    required int id,
    required int userId,
    required int? offerId,
    required String action,
    required Map<String, dynamic> details,
    required String ipAddress,
    required DateTime createdAt,
  }) : _id = id,
       _userId = userId,
       _offerId = offerId,
       _action = action,
       _details = details,
       _ipAddress = ipAddress,
       _createdAt = createdAt;

  ActivityRecord.fromJson(Map json)
    : _id = json['id'] ?? 0,
      _userId = json['userId'] ?? 0,
      _offerId = json['offerId'] as int?,
      _action = json['action']?.toString() ?? '',
      _details = json['details'] is Map
          ? Map<String, dynamic>.from(json['details'] as Map)
          : <String, dynamic>{},
      _ipAddress = json['ipAddress']?.toString() ?? '',
      _createdAt =
          DateTime.tryParse(json['createdAt']?.toString() ?? '') ??
          DateTime.now();

  Map<String, dynamic> toJson() => {
    'id': _id,
    'userId': _userId,
    'offerId': _offerId,
    'action': _action,
    'details': _details,
    'ipAddress': _ipAddress,
    'createdAt': _createdAt.toIso8601String(),
  };
}
