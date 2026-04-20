import 'package:flutter/material.dart';
import 'package:frontend/Models/cart_item.dart';
import 'package:frontend/Models/category.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Models/seller.dart';
import 'package:frontend/Presentation/Widgets/cart.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';

class CartPage extends StatelessWidget {
  final List<CartItem> _cartItems = [
    CartItem(
      id: 1,
      offer: Offer(
        title: "iPhone 17",
        description: "Nowy iPhone 17 w super cenie",
        price: 3000.0,
        images: ["https://picsum.photos/seed/abc/800/600"],
        seller: Seller(name: "John", rating: 5.0),
        category: Category(
          name: "Elektronika",
          description: "Artykuły elektroniczne",
        ),
        tags: ["telefon"],
        properties: [("Stan", "Nowy")],
        availability: 4,
        status: "DRAFT",
        createdAt: DateTime.now(),
        updatedAt: DateTime.now(),
      ),
      quantity: 1,
      addedAt: DateTime.now(),
    ),
    CartItem(
      id: 2,
      offer: Offer(
        title: "Acer Predator",
        description: "Super laptop firmy Acer",
        price: 4000.0,
        images: ["https://picsum.photos/seed/abc/800/600"],
        seller: Seller(name: "John", rating: 5.0),
        category: Category(
          name: "Elektronika",
          description: "Artykuły elektroniczne",
        ),
        tags: ["laptop"],
        properties: [("Stan", "Nowy")],
        availability: 4,
        status: "DRAFT",
        createdAt: DateTime.now(),
        updatedAt: DateTime.now(),
      ),
      quantity: 1,
      addedAt: DateTime.now(),
    ),
  ];

  CartPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      body: Padding(
        padding: EdgeInsetsGeometry.all(24),
        child: Cart(cartItems: _cartItems),
      ),
    );
  }
}
