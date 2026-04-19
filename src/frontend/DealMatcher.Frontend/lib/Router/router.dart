import 'package:frontend/Presentation/Pages/add_offer_page.dart';
import 'package:frontend/Presentation/Pages/main_page.dart';
import 'package:frontend/Presentation/Pages/register_page.dart';
import 'package:frontend/Presentation/Pages/login_page.dart';
import 'package:frontend/Presentation/Pages/search_offers_page.dart';
import 'package:go_router/go_router.dart';

final router = GoRouter(
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => MainPage(),
      routes: [
        GoRoute(path: 'register', builder: (context, state) => RegisterPage()),
        GoRoute(path: 'login', builder: (context, state) => LoginPage()),
        GoRoute(path: 'offer', builder: (context, state) => AddOfferPage()),
        GoRoute(
          path: 'search',
          builder: (context, state) => SearchOffersPage(),
        ),
      ],
    ),
  ],
  routerNeglect: false,
);
