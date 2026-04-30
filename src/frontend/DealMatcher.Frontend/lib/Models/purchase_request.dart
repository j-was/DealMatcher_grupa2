class PurchaseRequest {
  final int offerId;
  final String deliveryMethodId;
  final String paymentMethodId;
  final int quantity;

  const PurchaseRequest({
    required this.offerId,
    required this.deliveryMethodId,
    required this.paymentMethodId,
    required this.quantity,
  });

  Map<String, dynamic> toJson() {
    return {
      'offerId': offerId,
      'deliveryMethodId': deliveryMethodId,
      'paymentMethodId': paymentMethodId,
      'quantity': quantity,
    };
  }
}
