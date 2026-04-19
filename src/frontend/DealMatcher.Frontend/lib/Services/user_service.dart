import 'dart:convert';

import 'package:frontend/Models/user.dart';
import 'package:frontend/Models/user_register_data.dart';
import 'package:frontend/Models/user_login_data.dart';
import 'package:frontend/Models/login_response.dart';
import 'package:frontend/Models/user_update_data.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:http/http.dart' as http;

class UserService {
  static const baseUrl = String.fromEnvironment('API_URL');
  final AuthService _authService = AuthService.instance;

  Map<String, String> _jsonHeaders({bool authorized = false}) {
    final headers = <String, String>{
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    };

    if (authorized) {
      headers.addAll(_authService.authHeaders());
    }

    return headers;
  }

  Future<User> registerUser(UserRegisterData urd) async {
    final uri = Uri.parse('$baseUrl/v1/users/register');

    final response = await http.post(
      uri,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: jsonEncode(urd.toJson()),
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as Map<String, dynamic>;
      return User.fromJson(json);
    }

    throw Exception(
      'Nie udało się zarejestrować użytkownika.'
      'Status: ${response.statusCode}',
    );
  }

  Future<LoginResponse> loginUser(UserLoginData uld) async {
    final uri = Uri.parse('$baseUrl/v1/users/login');

    final response = await http.post(
      uri,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: jsonEncode(uld.toJson()),
    );

    if (response.statusCode == 200) {
      final Map<String, dynamic> json =
          jsonDecode(response.body) as Map<String, dynamic>;
      return LoginResponse.fromJson(json);
    }

    if (response.statusCode == 401) {
      throw Exception('Nieprawidłowy email lub hasło.');
    }

    if (response.statusCode == 403) {
      throw Exception('Użytkownik jest zablokowany.');
    }

    throw Exception(
      'Nie udało się zalogować. '
      'Status: ${response.statusCode}',
    );
  }

  Future<User> getCurrentUser() async {
    final uri = Uri.parse('$baseUrl/v1/users/me');
    final response = await http.get(
      uri,
      headers: _jsonHeaders(authorized: true),
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as Map;
      final user = User.fromJson(json);
      await _authService.updateCurrentUser(user);
      return user;
    }

    if (response.statusCode == 401) {
      await _authService.clearSession();
      throw Exception('Sesja wygasła. Zaloguj się ponownie.');
    }

    throw Exception(
      'Nie udało się pobrać profilu. Status: ${response.statusCode}',
    );
  }

  Future<User> updateCurrentUser(UserUpdateData data) async {
    final uri = Uri.parse('$baseUrl/v1/users/me');
    final response = await http.put(
      uri,
      headers: _jsonHeaders(authorized: true),
      body: jsonEncode(data.toJson()),
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as Map;
      final user = User.fromJson(json);
      await _authService.updateCurrentUser(user);
      return user;
    }

    if (response.statusCode == 400) {
      throw Exception('Niepoprawne dane profilu.');
    }

    if (response.statusCode == 401) {
      await _authService.clearSession();
      throw Exception('Sesja wygasła. Zaloguj się ponownie.');
    }

    throw Exception(
      'Nie udało się zapisać profilu. Status: ${response.statusCode}',
    );
  }

  Future<void> deleteCurrentUser() async {
    final uri = Uri.parse('$baseUrl/v1/users/me');
    final response = await http.delete(
      uri,
      headers: _jsonHeaders(authorized: true),
    );

    if (response.statusCode == 200 || response.statusCode == 204) {
      await _authService.clearSession();
      return;
    }

    if (response.statusCode == 401) {
      await _authService.clearSession();
      throw Exception('Sesja wygasła. Zaloguj się ponownie.');
    }

    throw Exception(
      'Nie udało się usunąć konta. Status: ${response.statusCode}',
    );
  }
}
