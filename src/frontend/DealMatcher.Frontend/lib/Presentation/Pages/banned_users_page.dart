import 'package:flutter/material.dart';
import 'package:frontend/Models/ban.dart';
import 'package:frontend/Models/user.dart';
import 'package:frontend/Presentation/Widgets/ban_detail_card.dart';
import 'package:frontend/Presentation/Widgets/ban_list_tile.dart';
import 'package:frontend/Services/admin_service.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:frontend/Services/ban_service.dart';

class BannedUsersPage extends StatefulWidget {
  const BannedUsersPage({super.key});

  @override
  State<BannedUsersPage> createState() => _BannedUsersPageState();
}

class _BannedUsersPageState extends State<BannedUsersPage> {
  final BanService _banService = BanService();
  final AdminService _adminService = AdminService();

  List<Ban> _bans = [];
  Ban? _selectedBan;
  User? _selectedUser;
  String? _error;
  bool _loadingList = true;
  bool _loadingDetails = false;

  static const bool _useMockData = false;

  static final List<Ban> _mockBans = [
    Ban.fromJson({
      'id': 1,
      'userId': 101,
      'reason': 'Spam w wiadomościach i ofertach',
      'issuedBy': 1,
      'issuedAt': '2026-05-10T12:30:00.000Z',
      'expiresAt': null,
      'isActive': true,
    }),
    Ban.fromJson({
      'id': 2,
      'userId': 102,
      'reason': 'Naruszenie regulaminu serwisu',
      'issuedBy': 1,
      'issuedAt': '2026-05-11T08:15:00.000Z',
      'expiresAt': '2026-06-11T08:15:00.000Z',
      'isActive': true,
    }),
    Ban.fromJson({
      'id': 3,
      'userId': 103,
      'reason': 'Podejrzana aktywność',
      'issuedBy': 2,
      'issuedAt': '2026-05-12T18:45:00.000Z',
      'expiresAt': null,
      'isActive': true,
    }),
  ];

  static final Map<int, User> _mockUsersById = {
    101: User.fromJson({
      'id': 101,
      'email': 'jan.kowalski@example.com',
      'name': 'Jan',
      'surname': 'Kowalski',
      'status': 'BANNED',
      'createdAt': '2025-10-01T09:00:00.000Z',
    }),
    102: User.fromJson({
      'id': 102,
      'email': 'anna.nowak@example.com',
      'name': 'Anna',
      'surname': 'Nowak',
      'status': 'BANNED',
      'createdAt': '2025-11-04T14:20:00.000Z',
    }),
    103: User.fromJson({
      'id': 103,
      'email': 'piotr.zielinski@example.com',
      'name': 'Piotr',
      'surname': 'Zieliński',
      'status': 'BANNED',
      'createdAt': '2025-12-15T07:30:00.000Z',
    }),
  };

  @override
  void initState() {
    super.initState();
    _loadBans();
  }

  String? _errorMessage(Object error) {
    final text = error.toString();
    return text.startsWith('Exception: ') ? text.substring(11) : text;
  }

  Future<void> _loadBans() async {
    if (!mounted) return;

    setState(() {
      _loadingList = true;
      _error = null;
    });

    try {
      final bans = _useMockData
          ? _mockBans
          : await _banService.getBans(active: true);

      if (!mounted) return;

      setState(() {
        _bans = bans;
        _selectedBan = bans.isNotEmpty ? bans.first : null;
      });

      if (_selectedBan != null) {
        await _loadBanDetails(_selectedBan!);
      } else {
        if (mounted) {
          setState(() {
            _selectedUser = null;
          });
        }
      }
    } catch (error) {
      if (!mounted) return;
      setState(() {
        _error = _errorMessage(error);
        _bans = [];
        _selectedBan = null;
        _selectedUser = null;
      });
    } finally {
      if (mounted) {
        setState(() {
          _loadingList = false;
        });
      }
    }
  }

  Future<void> _loadBanDetails(Ban ban) async {
    if (!mounted) return;

    setState(() {
      _loadingDetails = true;
      _selectedBan = ban;
      _selectedUser = null;
      _error = null;
    });

    try {
      if (_useMockData) {
        final user = _mockUsersById[ban.userId];
        if (!mounted) return;
        setState(() {
          _selectedUser = user;
        });
        return;
      }

      final detailedBan = await _banService.getBanDetails(ban.id);
      final user = await _adminService.getUserDetails(detailedBan.userId);

      if (!mounted) return;
      setState(() {
        _selectedBan = detailedBan;
        _selectedUser = user;
      });
    } catch (error) {
      if (!mounted) return;
      setState(() {
        _error = _errorMessage(error);
      });
    } finally {
      if (mounted) {
        setState(() {
          _loadingDetails = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return AnimatedBuilder(
      animation: AuthService.instance,
      builder: (context, _) {
        if (!AuthService.instance.isAdmin) {
          return const Scaffold(
            body: Center(
              child: Text('Brak uprawnień do wyświetlenia tej strony.'),
            ),
          );
        }

        return Scaffold(
          appBar: AppBar(
            title: const Text('Zbanowani użytkownicy'),
            actions: [
              IconButton(
                onPressed: _loadingList ? null : _loadBans,
                icon: const Icon(Icons.refresh),
                tooltip: 'Odśwież',
              ),
            ],
          ),
          body: SafeArea(
            child: Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                children: [
                  if (_error != null) ...[
                    MaterialBanner(
                      content: Text(_error!),
                      actions: [
                        TextButton(
                          onPressed: () => setState(() => _error = null),
                          child: const Text('Zamknij'),
                        ),
                      ],
                    ),
                    const SizedBox(height: 12),
                  ],
                  Card(
                    child: Padding(
                      padding: const EdgeInsets.all(16),
                      child: Row(
                        children: [
                          const Icon(Icons.block),
                          const SizedBox(width: 12),
                          Expanded(
                            child: Text(
                              'Aktywne bany: ${_bans.length}',
                              style: Theme.of(context).textTheme.titleMedium,
                            ),
                          ),
                          if (_useMockData) const Chip(label: Text('MOCK')),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: 16),
                  Expanded(
                    child: _loadingList
                        ? const Center(child: CircularProgressIndicator())
                        : _bans.isEmpty
                        ? const Center(child: Text('Brak aktywnych banów.'))
                        : ListView.builder(
                            itemCount: _bans.length,
                            itemBuilder: (context, index) {
                              final ban = _bans[index];
                              final selected = _selectedBan?.id == ban.id;

                              return Padding(
                                padding: const EdgeInsets.only(bottom: 12),
                                child: Column(
                                  crossAxisAlignment:
                                      CrossAxisAlignment.stretch,
                                  children: [
                                    BanListTile(
                                      ban: ban,
                                      selected: selected,
                                      onTap: () {
                                        setState(() {
                                          _selectedBan = selected ? null : ban;
                                          _selectedUser = null;
                                        });

                                        if (_selectedBan != null) {
                                          _loadBanDetails(_selectedBan!);
                                        }
                                      },
                                    ),
                                    AnimatedCrossFade(
                                      firstChild: const SizedBox.shrink(),
                                      secondChild: Padding(
                                        padding: const EdgeInsets.only(top: 8),
                                        child: BanDetailCard(
                                          ban: _selectedBan,
                                          user: _selectedUser,
                                          loadingUser: _loadingDetails,
                                        ),
                                      ),
                                      crossFadeState: selected
                                          ? CrossFadeState.showSecond
                                          : CrossFadeState.showFirst,
                                      duration: const Duration(
                                        milliseconds: 200,
                                      ),
                                    ),
                                  ],
                                ),
                              );
                            },
                          ),
                  ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
}
