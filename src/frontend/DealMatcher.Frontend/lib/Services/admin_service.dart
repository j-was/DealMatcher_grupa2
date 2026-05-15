import 'dart:convert';
import 'dart:io';

import 'package:frontend/Models/user.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:http/http.dart' as http;

class AdminService {
  static const String baseUrl = String.fromEnvironment('API_URL');

  Future<User> getUserDetails(int userId) async {
    try {
      final uri = Uri.parse('$baseUrl/v1/admin/users/$userId');
      final response = await http.get(
        uri,
        headers: AuthService.instance.authHeaders(),
      );

      if (response.statusCode == 200) {
        final json = jsonDecode(response.body) as Map;
        return User.fromJson(json);
      }

      if (response.statusCode == 401) {
        await AuthService.instance.clearSession();
        throw Exception('Sesja wygasła. Zaloguj się ponownie.');
      }

      if (response.statusCode == 403) {
        throw Exception('Brak uprawnień administratora.');
      }

      if (response.statusCode == 404) {
        throw Exception('Użytkownik nie został znaleziony.');
      }

      throw Exception(
        'Nie udało się pobrać danych użytkownika. Status: ${response.statusCode}',
      );
    } on SocketException {
      throw Exception(
        'Brak odpowiedzi serwera podczas pobierania danych użytkownika.',
      );
    } catch (error) {
      throw Exception('Nie udało się pobrać danych użytkownika: $error');
    }
  }
}
