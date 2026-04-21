import 'package:flutter/material.dart';
import 'package:frontend/Models/cart_item.dart';
import 'package:frontend/Models/cart_total.dart';
import 'package:frontend/Presentation/Widgets/cart.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Services/cart_service.dart';

class CartPage extends StatefulWidget {
  const CartPage({super.key});

  @override
  State createState() => _CartPageState();
}

class _CartPageState extends State<CartPage> {
  final CartService _cartService = CartService.instance;

  late Future<List<CartItem>> _cartFuture;
  late Future<CartTotal> _totalFuture;

  @override
  void initState() {
    super.initState();
    _reload();
  }

  void _reload() {
    _cartFuture = _cartService.getCartItems();
    _totalFuture = _cartService.getCartTotal();
  }

  Future<void> _refresh() async {
    setState(_reload);
    await _cartFuture;
    try {
      await _totalFuture;
    } catch (_) {}
  }

  Future<void> _changeQuantity(int cartItemId, int quantity) async {
    try {
      await _cartService.updateCartItemQuantity(
        cartItemId: cartItemId,
        quantity: quantity,
      );
      setState(_reload);
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Nie udało się zmienić ilości: $e')),
      );
      rethrow;
    }
  }

  Future<void> _removeItem(int cartItemId) async {
    try {
      await _cartService.removeCartItem(cartItemId);
      setState(_reload);
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Usunięto z koszyka')));
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Nie udało się usunąć pozycji: $e')),
      );
      rethrow;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      body: Padding(
        padding: const EdgeInsets.all(24),
        child: FutureBuilder<List<CartItem>>(
          future: _cartFuture,
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return const Center(child: CircularProgressIndicator());
            }

            if (snapshot.hasError) {
              return Center(
                child: Text('Błąd pobierania koszyka: ${snapshot.error}'),
              );
            }

            final items = snapshot.data ?? <CartItem>[];
            final fallbackTotal = items.fold<double>(
              0.0,
              (sum, item) => sum + item.offer.price * item.quantity,
            );

            return Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                FutureBuilder<CartTotal>(
                  future: _totalFuture,
                  builder: (context, totalSnapshot) {
                    final total =
                        totalSnapshot.data ??
                        CartTotal(totalPrice: fallbackTotal, currency: 'PLN');

                    return Text(
                      'Suma: ${total.totalPrice.toStringAsFixed(2)} ${total.currency}',
                      style: Theme.of(context).textTheme.headlineSmall,
                    );
                  },
                ),
                const SizedBox(height: 16),
                Expanded(
                  child: RefreshIndicator(
                    onRefresh: _refresh,
                    child: items.isEmpty
                        ? ListView(
                            physics: const AlwaysScrollableScrollPhysics(),
                            children: const [
                              SizedBox(height: 120),
                              Center(child: Text('Koszyk jest pusty')),
                            ],
                          )
                        : Cart(
                            cartItems: items,
                            onQuantityChanged: _changeQuantity,
                            onDelete: _removeItem,
                          ),
                  ),
                ),
              ],
            );
          },
        ),
      ),
    );
  }
}
