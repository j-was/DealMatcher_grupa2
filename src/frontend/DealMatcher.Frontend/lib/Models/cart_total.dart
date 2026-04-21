class CartTotal {
  final double totalPrice;
  final String currency;

  const CartTotal({required this.totalPrice, required this.currency});

  factory CartTotal.fromJson(Map json) {
    return CartTotal(
      totalPrice: (json['totalPrice'] as num?)?.toDouble() ?? 0.0,
      currency: json['currency']?.toString() ?? 'PLN',
    );
  }
}
