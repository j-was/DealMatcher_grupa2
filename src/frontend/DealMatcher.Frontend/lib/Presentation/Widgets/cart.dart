import 'package:flutter/material.dart';
import 'package:frontend/Models/cart_item.dart';
import 'package:frontend/Presentation/Widgets/offer_in_cart.dart';
import 'package:frontend/Presentation/Widgets/seperated_widget.dart';

class Cart extends StatefulWidget {
  final List<CartItem> cartItems;
  const Cart({super.key, required this.cartItems});
  @override
  State<Cart> createState() => _CartState();
}

class _CartState extends State<Cart> {
  @override
  Widget build(BuildContext context) {
    return ListView(
      children: [
        ...List.generate(widget.cartItems.length, (i) {
          return SeparatedWidget(
            widget: OfferInCart(cartItem: widget.cartItems[i]),
          );
        }),
      ],
    );
  }
}
