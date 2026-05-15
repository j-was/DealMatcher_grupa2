class BanRequest {
  final int _userId;
  final String _reason;
  final DateTime? _expiresAt;

  int get userId => _userId;
  String get reason => _reason;
  DateTime? get expiresAt => _expiresAt;

  const BanRequest({
    required int userId,
    required String reason,
    DateTime? expiresAt,
  }) : _userId = userId,
       _reason = reason,
       _expiresAt = expiresAt;

  Map<String, dynamic> toJson() {
    return {
      'userId': _userId,
      'reason': _reason,
      if (_expiresAt != null) 'expiresAt': _expiresAt.toIso8601String(),
    };
  }
}
