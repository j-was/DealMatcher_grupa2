import 'package:frontend/Models/user.dart';

class AdminUsersResponse {
  final List<User> _items;
  final int _total;
  final int _page;
  final int _pages;

  List<User> get items => _items;
  int get total => _total;
  int get page => _page;
  int get pages => _pages;

  const AdminUsersResponse({
    required List<User> items,
    required int total,
    required int page,
    required int pages,
  }) : _items = items,
       _total = total,
       _page = page,
       _pages = pages;

  AdminUsersResponse.fromJson(Map json)
    : _items = json['items'] is List
          ? (json['items'] as List)
                .map((item) => User.fromJson(item as Map<String, dynamic>))
                .toList()
          : [],
      _total = json['total'] ?? 0,
      _page = json['page'] ?? 0,
      _pages = json['pages'] ?? 0;
}
