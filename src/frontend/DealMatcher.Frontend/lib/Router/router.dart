import 'package:frontend/Models/offer.dart';
import 'package:frontend/Presentation/Pages/add_offer_page.dart';
import 'package:frontend/Presentation/Pages/main_page.dart';
import 'package:frontend/Presentation/Pages/register_page.dart';
import 'package:frontend/Presentation/Pages/login_page.dart';
import 'package:frontend/Presentation/Pages/search_offers_page.dart';
import 'package:frontend/Presentation/Pages/searched_offers_page.dart';
import 'package:go_router/go_router.dart';
import 'package:frontend/Presentation/Pages/profile_page.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:frontend/Presentation/Pages/my_offers_page.dart';
import 'package:frontend/Presentation/Pages/offer_edit_page.dart';

final router = GoRouter(
  refreshListenable: AuthService.instance,
  redirect: (context, state) {
    final isLoggedIn = AuthService.instance.isAuthenticated;
    final location = state.matchedLocation;

    final isAuthRoute = location == '/login' || location == '/register';
    final isProtectedRoute =
        location == '/offer' ||
        location == '/profile' ||
        location.startsWith('/my-offers');

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
          path: 'my-offers',
          builder: (context, state) => const MyOffersPage(),
          routes: [
            GoRoute(
              path: ':offerId',
              builder: (context, state) {
                final offerId = int.parse(state.pathParameters['offerId']!);
                return OfferEditPage(offerId: offerId);
              },
            ),
          ],
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
      ],
    ),
  ],
  routerNeglect: false,
);
