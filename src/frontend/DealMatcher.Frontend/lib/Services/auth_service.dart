import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:frontend/Models/login_response.dart';
import 'package:frontend/Models/user.dart';
import 'package:shared_preferences/shared_preferences.dart';

class AuthService extends ChangeNotifier {
  AuthService._();

  static final AuthService instance = AuthService._();

  static const String _accessTokenKey = 'auth.accessToken';
  static const String _userKey = 'auth.user';

  final SharedPreferencesAsync _prefs = SharedPreferencesAsync();

  String? _accessToken;
  User? _currentUser;

  bool get isAuthenticated => (_accessToken ?? '').isNotEmpty;
  String? get accessToken => _accessToken;
  User? get currentUser => _currentUser;

  bool get isAdmin => _currentUser?.status == 'ADMIN';

  Future<void> restoreSession() async {
    try {
      final token = await _prefs.getString(_accessTokenKey);
      final userJson = await _prefs.getString(_userKey);

      _accessToken = token;
      _currentUser = _decodeUser(userJson);

      if (!isAuthenticated || _currentUser == null) {
        _accessToken = null;
        _currentUser = null;
        await _prefs.remove(_accessTokenKey);
        await _prefs.remove(_userKey);
      }
    } catch (_) {
      await clearSession();
      return;
    }

    notifyListeners();
  }

  Future<void> saveSession(LoginResponse response) async {
    _accessToken = response.accessToken;
    _currentUser = response.user;

    await _prefs.setString(_accessTokenKey, response.accessToken);
    await _prefs.setString(_userKey, jsonEncode(response.user.toJson()));

    notifyListeners();
  }

  Future<void> updateCurrentUser(User user) async {
    _currentUser = user;
    await _prefs.setString(_userKey, jsonEncode(user.toJson()));
    notifyListeners();
  }

  Future<void> clearSession() async {
    _accessToken = null;
    _currentUser = null;

    await _prefs.remove(_accessTokenKey);
    await _prefs.remove(_userKey);

    notifyListeners();
  }

  Map<String, String> authHeaders() {
    if (!isAuthenticated) {
      throw StateError('Brak tokenu autoryzacji.');
    }

    return {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'Bearer $_accessToken',
    };
  }

  User? _decodeUser(String? rawJson) {
    if (rawJson == null || rawJson.isEmpty) {
      return null;
    }

    try {
      final decoded = jsonDecode(rawJson);
      if (decoded is Map) {
        return User.fromJson(decoded);
      }
    } catch (_) {
      return null;
    }

    return null;
  }
}
