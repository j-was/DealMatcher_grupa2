import 'package:flutter/material.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Presentation/Widgets/offer_view.dart';

class SearchedOfferSection extends StatelessWidget {
  final Offer offer;

  const SearchedOfferSection({super.key, required this.offer});

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(child: OfferView(offer: offer));
  }
}
