import 'dart:convert';

import 'package:frontend/Models/cart_item.dart';
import 'package:frontend/Models/cart_total.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:http/http.dart' as http;

class CartService {
  CartService._();

  static final CartService instance = CartService._();

  static const String baseUrl = String.fromEnvironment('API_URL');

  Uri _endpoint(String path) {
    final root = baseUrl.trim();
    if (root.isEmpty) {
      throw StateError('API_URL is not configured.');
    }
    return Uri.parse('$root$path');
  }

  Future<List<CartItem>> getCartItems() async {
    final response = await http.get(
      _endpoint('/cart/items'),
      headers: AuthService.instance.authHeaders(),
    );

    if (response.statusCode == 200) {
      final decoded = jsonDecode(response.body) as List<dynamic>;
      return decoded
          .map((item) => CartItem.fromJson(item as Map<String, dynamic>))
          .toList();
    }

    if (response.statusCode == 204) {
      return <CartItem>[];
    }

    throw Exception(
      'Nie udało się pobrać koszyka. Status: ${response.statusCode}',
    );
  }

  Future<CartItem> addToCart({required int offerId, int quantity = 1}) async {
    final response = await http.post(
      _endpoint('/cart/items'),
      headers: AuthService.instance.authHeaders(),
      body: jsonEncode({'offerId': offerId, 'quantity': quantity}),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      final decoded = jsonDecode(response.body) as Map<String, dynamic>;
      return CartItem.fromJson(decoded);
    }

    throw Exception(
      'Nie udało się dodać oferty do koszyka. Status: ${response.statusCode}',
    );
  }

  Future<CartItem> updateCartItemQuantity({
    required int cartItemId,
    required int quantity,
  }) async {
    final response = await http.patch(
      _endpoint('/cart/items/$cartItemId'),
      headers: AuthService.instance.authHeaders(),
      body: jsonEncode({'quantity': quantity}),
    );

    if (response.statusCode == 200) {
      final decoded = jsonDecode(response.body) as Map<String, dynamic>;
      return CartItem.fromJson(decoded);
    }

    throw Exception(
      'Nie udało się zmienić ilości. Status: ${response.statusCode}',
    );
  }

  Future<void> removeCartItem(int cartItemId) async {
    final response = await http.delete(
      _endpoint('/cart/items/$cartItemId'),
      headers: AuthService.instance.authHeaders(),
    );

    if (response.statusCode == 200 || response.statusCode == 204) {
      return;
    }

    throw Exception(
      'Nie udało się usunąć pozycji z koszyka. Status: ${response.statusCode}',
    );
  }

  Future<CartTotal> getCartTotal() async {
    final response = await http.get(
      _endpoint('/cart/total'),
      headers: AuthService.instance.authHeaders(),
    );

    if (response.statusCode == 200) {
      final decoded = jsonDecode(response.body) as Map;
      return CartTotal.fromJson(decoded);
    }

    throw Exception(
      'Nie udało się pobrać sumy koszyka. Status: ${response.statusCode}',
    );
  }
}
