class Ban {
  final int _id;
  final int _userId;
  final String _reason;
  final int _issuedBy;
  final DateTime _issuedAt;
  final DateTime? _expiresAt;
  final bool _isActive;

  int get id => _id;
  int get userId => _userId;
  String get reason => _reason;
  int get issuedBy => _issuedBy;
  DateTime get issuedAt => _issuedAt;
  DateTime? get expiresAt => _expiresAt;
  bool get isActive => _isActive;

  const Ban({
    int id = -1,
    required int userId,
    required String reason,
    required int issuedBy,
    required DateTime issuedAt,
    DateTime? expiresAt,
    required bool isActive,
  }) : _id = id,
       _userId = userId,
       _reason = reason,
       _issuedBy = issuedBy,
       _issuedAt = issuedAt,
       _expiresAt = expiresAt,
       _isActive = isActive;

  Ban copyWith({
    int? id,
    int? userId,
    String? reason,
    int? issuedBy,
    DateTime? issuedAt,
    DateTime? expiresAt,
    bool? isActive,
  }) {
    return Ban(
      id: id ?? _id,
      userId: userId ?? _userId,
      reason: reason ?? _reason,
      issuedBy: issuedBy ?? _issuedBy,
      issuedAt: issuedAt ?? _issuedAt,
      expiresAt: expiresAt ?? _expiresAt,
      isActive: isActive ?? _isActive,
    );
  }

  Ban.fromJson(Map json)
    : _id = json['id'] ?? 0,
      _userId = json['userId'] ?? 0,
      _reason = json['reason'] ?? '',
      _issuedBy = json['issuedBy'] ?? 0,
      _issuedAt =
          DateTime.tryParse(json['issuedAt']?.toString() ?? '') ??
          DateTime.now(),
      _expiresAt = json['expiresAt'] == null
          ? null
          : DateTime.tryParse(json['expiresAt'].toString()),
      _isActive = json['isActive'] ?? false;

  Map<String, dynamic> toJson() {
    return {
      'id': _id,
      'userId': _userId,
      'reason': _reason,
      'issuedBy': _issuedBy,
      'issuedAt': _issuedAt.toIso8601String(),
      'expiresAt': _expiresAt?.toIso8601String(),
      'isActive': _isActive,
    };
  }
}
