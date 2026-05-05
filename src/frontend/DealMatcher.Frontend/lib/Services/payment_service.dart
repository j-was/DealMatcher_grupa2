import 'dart:convert';

import 'package:frontend/Models/payment_method.dart';
import 'package:frontend/Models/purchase_request.dart';
import 'package:http/http.dart' as http;

class PaymentService {
  static const String baseUrl = String.fromEnvironment('API_URL');

  Future<List<PaymentMethod>> getMethods() async {
    final uri = Uri.parse('$baseUrl/v1/purchases/payment-methods');

    final response = await http.get(
      uri,
      headers: {'Accept': 'application/json'},
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body);

      final methods = (json as List)
          .map((jsonCat) => PaymentMethod.fromJson(jsonCat))
          .toList();
      return methods;
    }

    return List.empty();
  }

  Future<String?> initialize(PurchaseRequest req) async {
    final uri = Uri.parse('$baseUrl/v1/purchases/initialize');

    final response = await http.post(
      uri,
      headers: {'Accept': 'application/json'},
      body: req.toJson(),
    );

    if (response.statusCode == 303) {
      return null;
    }

    return "Błąd ${response.statusCode}. Spróbuj ponownie";
  }
}
