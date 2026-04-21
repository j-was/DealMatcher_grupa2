import 'package:flutter/material.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Presentation/Widgets/main_side_menu.dart';
import 'package:frontend/Presentation/Widgets/reaction_buttons.dart';
import 'package:frontend/Presentation/Widgets/searched_offer_section.dart';
import 'package:frontend/Services/cart_service.dart';
import 'package:frontend/Services/offer_service.dart';
import 'package:go_router/go_router.dart';

class MainPage extends StatefulWidget {
  const MainPage({super.key});
  @override
  State<MainPage> createState() => _MainPageState();
}

class _MainPageState extends State<MainPage> {
  List<Offer> _offers = [];
  int _currIdx = 0;
  bool _isLoading = true;
  bool _isAddingToCart = false;

  @override
  void initState() {
    super.initState();
    _loadOffers();
  }

  Future<void> _loadOffers() async {
    try {
      final offers = await OfferService().searchOffers({
        'categoryId': null,
        'minPrice': null,
        'maxPrice': null,
        'tags': null,
        'properties': null,
        'searchPhrase': null,
        'limit': 50,
      });
      setState(() {
        _offers = offers;
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _isLoading = false;
      });
    }
  }

  void nextOffer() {
    if (_currIdx < _offers.length) {
      setState(() {
        _currIdx++;
      });
    }
  }

  Future<void> _addToCart() async {
    if (_currIdx >= _offers.length || _isAddingToCart) return;
    final offer = _offers[_currIdx];

    setState(() {
      _isAddingToCart = true;
    });

    try {
      await CartService.instance.addToCart(offerId: offer.id);
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Dodano do koszyka')));
    } on StateError {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Zaloguj się, aby dodać do koszyka')),
      );
      context.go('/login');
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Nie udało się dodać do koszyka: $e')),
      );
    } finally {
      if (mounted) {
        setState(() {
          _isAddingToCart = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      drawer: const MainSideMenu(),
      body: Padding(
        padding: EdgeInsetsGeometry.all(24),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            if (_isLoading)
              const Expanded(child: Center(child: CircularProgressIndicator()))
            else if (_currIdx < _offers.length)
              Expanded(child: SearchedOfferSection(offer: _offers[_currIdx]))
            else
              const Expanded(
                child: Center(
                  child: Text(
                    'Brak ofert',
                    style: TextStyle(color: Colors.white54, fontSize: 32),
                  ),
                ),
              ),
            ReactionButtons(
              onDislike: nextOffer,
              onLike: nextOffer,
              onSuperlike: _currIdx < _offers.length ? _addToCart : null,
            ),
          ],
        ),
      ),
    );
  }
}
