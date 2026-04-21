import 'package:flutter/material.dart';
import 'package:frontend/Models/cart_item.dart';
import 'package:frontend/Presentation/Widgets/offer_in_cart.dart';
import 'package:frontend/Presentation/Widgets/seperated_widget.dart';

class Cart extends StatefulWidget {
  final List<CartItem> cartItems;
  final Future<void> Function(int cartItemId, int quantity) onQuantityChanged;
  final Future<void> Function(int cartItemId) onDelete;

  const Cart({
    super.key,
    required this.cartItems,
    required this.onQuantityChanged,
    required this.onDelete,
  });

  @override
  State<Cart> createState() => _CartState();
}

class _CartState extends State<Cart> {
  late List<CartItem> _items;

  @override
  void initState() {
    super.initState();
    _items = List.of(widget.cartItems);
  }

  @override
  void didUpdateWidget(covariant Cart oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (oldWidget.cartItems != widget.cartItems) {
      _items = List.of(widget.cartItems);
    }
  }

  Future<void> _deleteItem(int cartItemId) async {
    await widget.onDelete(cartItemId);
    if (!mounted) return;
    setState(() {
      _items.removeWhere((item) => item.id == cartItemId);
    });
  }

  Future<void> _changeQuantity(int cartItemId, int quantity) async {
    await widget.onQuantityChanged(cartItemId, quantity);
    if (!mounted) return;
    setState(() {
      final index = _items.indexWhere((item) => item.id == cartItemId);
      if (index != -1) {
        _items[index] = CartItem(
          id: _items[index].id,
          offer: _items[index].offer,
          quantity: quantity,
          addedAt: _items[index].addedAt,
        );
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return ListView(
      children: [
        ...List.generate(_items.length, (i) {
          return SeparatedWidget(
            widget: OfferInCart(
              cartItem: _items[i],
              onQuantityChanged: _changeQuantity,
              onDelete: _deleteItem,
            ),
          );
        }),
      ],
    );
  }
}
