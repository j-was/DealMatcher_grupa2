import 'package:frontend/Presentation/Pages/add_offer_page.dart';
import 'package:frontend/Presentation/Pages/main_page.dart';
import 'package:frontend/Presentation/Pages/register_page.dart';
import 'package:go_router/go_router.dart';

final router = GoRouter(
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => MainPage(),
      routes: [
        GoRoute(path: 'register', builder: (context, state) => RegisterPage()),
        GoRoute(path: 'offer', builder: (context, state) => AddOfferPage()),
      ],
    ),
  ],
  routerNeglect: false,
);
