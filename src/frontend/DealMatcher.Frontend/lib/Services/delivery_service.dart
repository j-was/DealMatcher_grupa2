import 'dart:convert';

import 'package:frontend/Models/delivery_method.dart';
import 'package:http/http.dart' as http;

class DeliveryService {
  static const String baseUrl = String.fromEnvironment('API_URL');

  Future<List<DeliveryMethod>> getMethods() async {
    final uri = Uri.parse('$baseUrl/purchases/delivery-methods');

    final response = await http.get(
      uri,
      headers: {'Accept': 'application/json'},
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body);

      final methods = (json as List)
          .map((jsonCat) => DeliveryMethod.fromJson(jsonCat))
          .toList();
      return methods;
    }

    return List.empty();
  }
}
