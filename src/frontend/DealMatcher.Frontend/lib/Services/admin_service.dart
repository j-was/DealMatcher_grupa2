import 'dart:convert';
import 'dart:io';

import 'package:frontend/Models/admin_offers_response.dart';
import 'package:frontend/Models/admin_users_response.dart';
import 'package:frontend/Models/user.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:frontend/Models/activity_record.dart';
import 'package:http/http.dart' as http;

class AdminService {
  static const String baseUrl = String.fromEnvironment('API_URL');

  // Map<String, String> _headers() {
  //   return AuthService.instance.authHeaders();
  // }
  Map<String, String> _getJsonGetHeaders() {
    final authHeaders = AuthService.instance.authHeaders();
    return {
      'Accept': 'application/json',
      if (authHeaders['Authorization'] != null)
        'Authorization': authHeaders['Authorization']!,
    };
  }

  Future<AdminUsersResponse> getUsers({
    int page = 1,
    int limit = 20,
    String? status,
  }) async {
    final queryParameters = <String, String>{
      'page': page.toString(),
      'limit': limit.toString(),
    };

    if (status != null && status.isNotEmpty) {
      queryParameters['status'] = status;
    }

    final uri = Uri.parse(
      '$baseUrl/admin/users',
    ).replace(queryParameters: queryParameters);

    final authHeaders = AuthService.instance.authHeaders();

    final headers = <String, String>{
      if (authHeaders['Authorization'] != null)
        'Authorization': authHeaders['Authorization']!,
      'Accept': 'application/json',
    };

    final response = await http.get(uri, headers: headers);

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as Map;
      return AdminUsersResponse.fromJson(json);
    }

    if (response.statusCode == 401) {
      throw Exception('Unauthorized');
    }

    if (response.statusCode == 403) {
      throw Exception('Forbidden - admin only');
    }

    throw Exception(
      'Failed to load users. Status code: ${response.statusCode}. Body: ${response.body}',
    );
  }

  Future<AdminOffersResponse> getOffers({
    int page = 1,
    int limit = 20,
    String? status,
  }) async {
    final queryParameters = <String, String>{
      'page': page.toString(),
      'limit': limit.toString(),
    };

    if (status != null && status.isNotEmpty) {
      queryParameters['status'] = status;
    }

    final uri = Uri.parse(
      '$baseUrl/admin/offers',
    ).replace(queryParameters: queryParameters);

    final authHeaders = AuthService.instance.authHeaders();

    final headers = <String, String>{
      if (authHeaders['Authorization'] != null)
        'Authorization': authHeaders['Authorization']!,
      'Accept': 'application/json',
    };

    final response = await http.get(uri, headers: headers);

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as Map;
      return AdminOffersResponse.fromJson(json);
    }

    if (response.statusCode == 401) {
      throw Exception('Unauthorized');
    }

    if (response.statusCode == 403) {
      throw Exception('Forbidden - admin only');
    }

    throw Exception(
      'Failed to load offers. Status code: ${response.statusCode}. Body: ${response.body}',
    );
  }

  Future<List<ActivityRecord>> getUserActivity(
    int userId, {
    DateTime? from,
    DateTime? to,
  }) async {
    final queryParameters = <String, String>{};

    if (from != null) {
      queryParameters['from'] = from.toUtc().toIso8601String();
    }

    if (to != null) {
      queryParameters['to'] = to.toUtc().toIso8601String();
    }

    // final uri = Uri.parse('$baseUrl/admin/activity/user/$userId').replace(
    //   queryParameters: queryParameters.isEmpty ? null : queryParameters,
    // );

    final uri = Uri.parse('$baseUrl/admin/activity/user/$userId');
    final response = await http.get(uri, headers: _getJsonGetHeaders());

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as List;
      return json.map((item) => ActivityRecord.fromJson(item as Map)).toList();
    }

    if (response.statusCode == 204) {
      return [];
    }

    if (response.statusCode == 401) {
      throw Exception('Unauthorized');
    }

    if (response.statusCode == 403) {
      throw Exception('Forbidden - admin only');
    }

    if (response.statusCode == 404) {
      throw Exception('User not found');
    }

    throw Exception(
      'Failed to load user activity. Status code: ${response.statusCode}',
    );
  }

  Future<List<ActivityRecord>> getOfferActivity(
    int offerId, {
    DateTime? from,
    DateTime? to,
  }) async {
    final queryParameters = <String, String>{};

    if (from != null) {
      queryParameters['from'] = from.toUtc().toIso8601String();
    }

    if (to != null) {
      queryParameters['to'] = to.toUtc().toIso8601String();
    }

    // final uri = Uri.parse('$baseUrl/admin/activity/offer/$offerId').replace(
    //   queryParameters: queryParameters.isEmpty ? null : queryParameters,
    // );
    final uri = Uri.parse('$baseUrl/admin/activity/offer/$offerId');

    final response = await http.get(uri, headers: _getJsonGetHeaders());

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as List;
      return json.map((item) => ActivityRecord.fromJson(item as Map)).toList();
    }

    if (response.statusCode == 204) {
      return [];
    }

    if (response.statusCode == 401) {
      throw Exception('Unauthorized');
    }

    if (response.statusCode == 403) {
      throw Exception('Forbidden - admin only');
    }

    if (response.statusCode == 404) {
      throw Exception('Offer not found');
    }

    throw Exception(
      'Failed to load offer activity. Status code: ${response.statusCode}',
    );
  }

  Future<User> getUserDetails(int userId) async {
    try {
      final uri = Uri.parse('$baseUrl/admin/users/$userId');
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
