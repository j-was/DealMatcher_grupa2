import 'dart:convert';

import 'package:frontend/Models/category.dart';
import 'package:http/http.dart' as http;

class CategoryService {
  static const String baseUrl = String.fromEnvironment('API_URL');

  Future<List<Category>> getCategories() async {
    final uri = Uri.parse('$baseUrl/v1/categories');

    final response = await http.get(
      uri,
      headers: {'Accept': 'application/json'},
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body);

      final categories = (json as List)
          .map((jsonCat) => Category.fromJson(jsonCat))
          .toList();
      return categories;
    }

    return List.empty();
  }
}
