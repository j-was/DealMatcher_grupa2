import 'package:frontend/Models/offer.dart';
import 'package:frontend/Presentation/Pages/add_offer_page.dart';
import 'package:frontend/Presentation/Pages/cart_page.dart';
import 'package:frontend/Presentation/Pages/main_page.dart';
import 'package:frontend/Presentation/Pages/register_page.dart';
import 'package:frontend/Presentation/Pages/login_page.dart';
import 'package:frontend/Presentation/Pages/search_offers_page.dart';
import 'package:frontend/Presentation/Pages/searched_offers_page.dart';
import 'package:go_router/go_router.dart';
import 'package:frontend/Presentation/Pages/profile_page.dart';
import 'package:frontend/Services/auth_service.dart';

final router = GoRouter(
  refreshListenable: AuthService.instance,
  redirect: (context, state) {
    final isLoggedIn = AuthService.instance.isAuthenticated;
    final location = state.matchedLocation;

    final isAuthRoute = location == '/login' || location == '/register';
    final isProtectedRoute = location == '/offer' || location == '/profile';

    if (!isLoggedIn && isProtectedRoute) {
      return '/login';
    }

    if (isLoggedIn && isAuthRoute) {
      return '/profile';
    }

    return null;
  },
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => const MainPage(),
      routes: [
        GoRoute(path: 'login', builder: (context, state) => const LoginPage()),
        GoRoute(
          path: 'register',
          builder: (context, state) => const RegisterPage(),
        ),
        GoRoute(
          path: 'offer',
          builder: (context, state) => const AddOfferPage(),
        ),
        GoRoute(
          path: 'profile',
          builder: (context, state) => const ProfilePage(),
        ),
        GoRoute(
          path: 'search',
          builder: (context, state) => const SearchOffersPage(),
        ),
        GoRoute(
          path: 'searched',
          builder: (context, state) {
            final offers = state.extra as List<Offer>? ?? [];
            return SearchedOffersPage(offers: offers);
          },
        ),
        GoRoute(path: 'cart', builder: (context, state) => CartPage()),
      ],
    ),
  ],
  routerNeglect: false,
);
