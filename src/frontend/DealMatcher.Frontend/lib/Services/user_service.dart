import 'dart:convert';

import 'package:frontend/Models/user.dart';
import 'package:frontend/Models/user_register_data.dart';
import 'package:frontend/Models/user_login_data.dart';
import 'package:frontend/Models/login_response.dart';
import 'package:http/http.dart' as http;

class UserService {
  static const baseUrl = String.fromEnvironment('API_URL');

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
      final json = jsonDecode(response.body) as Map<String, dynamic>;
      if (jsonResponse.containsKey('value')) {
    final userData = jsonResponse['value'] as Map<String, dynamic>;
      return LoginResponse.fromJson(userData);
    }
    
    return LoginResponse.fromJson(jsonResponse);
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
}
