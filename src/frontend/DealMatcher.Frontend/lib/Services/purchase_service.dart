import 'dart:convert';

import 'package:frontend/Models/delivery_method.dart';
import 'package:frontend/Models/payment_method.dart';
import 'package:frontend/Models/purchase_request.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:http/http.dart' as http;

class PurchaseService {
  PurchaseService._();

  static final PurchaseService instance = PurchaseService._();

  static const String baseUrl = String.fromEnvironment('API_URL');

  Uri _endpoint(String path) {
    final root = baseUrl.trim();
    if (root.isEmpty) {
      throw StateError('API_URL is not configured.');
    }
    return Uri.parse('$root/v1$path');
  }

  Future<List<DeliveryMethod>> getDeliveryMethods() async {
    final response = await http.get(
      _endpoint('/purchases/delivery-methods'),
      headers: AuthService.instance.authHeaders(),
    );

    if (response.statusCode == 200) {
      final decoded = jsonDecode(response.body) as List<dynamic>;
      return decoded
          .map((item) => DeliveryMethod.fromJson(item as Map<String, dynamic>))
          .toList();
    }

    throw Exception(
      'Nie udało się pobrać metod dostawy. Status: ${response.statusCode}',
    );
  }

  Future<List<PaymentMethod>> getPaymentMethods() async {
    final response = await http.get(
      _endpoint('/purchases/payment-methods'),
      headers: AuthService.instance.authHeaders(),
    );

    if (response.statusCode == 200) {
      final decoded = jsonDecode(response.body) as List<dynamic>;
      return decoded
          .map((item) => PaymentMethod.fromJson(item as Map<String, dynamic>))
          .toList();
    }

    throw Exception(
      'Nie udało się pobrać metod płatności. Status: ${response.statusCode}',
    );
  }

  Future<Uri> initializePurchase(PurchaseRequest request) async {
    final response = await http.post(
      _endpoint('/purchases/initialize'),
      headers: {
        ...AuthService.instance.authHeaders(),
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: jsonEncode(request.toJson()),
    );

    if (response.statusCode == 200) {
      final decoded = jsonDecode(response.body) as Map<String, dynamic>;
      final redirectUrl = decoded['redirectUrl'] as String?;

      if (redirectUrl == null || redirectUrl.trim().isEmpty) {
        throw Exception('Brak adresu przekierowania do płatności.');
      }

      return Uri.parse(redirectUrl);
    }

    if (response.statusCode == 400) {
      throw Exception('Niepoprawne dane zamówienia.');
    }

    if (response.statusCode == 401) {
      throw Exception('Musisz być zalogowany, żeby złożyć zamówienie.');
    }

    if (response.statusCode == 404) {
      throw Exception('Nie znaleziono oferty.');
    }

    if (response.statusCode == 409) {
      throw Exception('Ta oferta nie jest już dostępna do zakupu.');
    }

    throw Exception(
      'Nie udało się zainicjalizować zamówienia. Status: ${response.statusCode}',
    );
  }

  Future<void> completePurchase(int userId) async {
    final response = await http.delete(
      _endpoint('/purchases/complete/$userId'),
      headers: AuthService.instance.authHeaders(),
    );

    if (response.statusCode == 200 || response.statusCode == 204) {
      return;
    }

    if (response.statusCode == 401) {
      throw Exception('Musisz być zalogowany, żeby dokończyć zakup.');
    }

    if (response.statusCode == 404) {
      throw Exception('Nie znaleziono zakupu do zakończenia.');
    }

    if (response.statusCode == 409) {
      throw Exception('Nie można zakończyć tego zakupu.');
    }

    throw Exception(
      'Nie udało się zakończyć zakupu. Status: ${response.statusCode}',
    );
  }
}
