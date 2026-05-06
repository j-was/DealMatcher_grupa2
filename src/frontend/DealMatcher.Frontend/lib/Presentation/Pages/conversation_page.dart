import 'package:flutter/material.dart';
import 'package:frontend/Models/conversation_detail.dart';
import 'package:frontend/Models/message.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:frontend/Services/conversation_realtime_service.dart';
import 'package:frontend/Services/conversation_service.dart';

class ConversationPage extends StatefulWidget {
  final int conversationId;

  const ConversationPage({super.key, required this.conversationId});

  @override
  State<ConversationPage> createState() => _ConversationPageState();
}

class _ConversationPageState extends State<ConversationPage> {
  final _conversationService = ConversationService();
  final TextEditingController _controller = TextEditingController();
  final _realtimeService = ConversationRealtimeService();

  late Future<ConversationDetail> _futureConversation;
  ConversationDetail? _conversation;
  bool _sending = false;

  Future<ConversationDetail> _loadConversation() async {
    final conversation = await _conversationService.getConversationDetail(
      widget.conversationId,
    );

    _conversation = conversation;

    await _realtimeService.connect(
      baseUrl: ConversationService.baseUrl,
      token: AuthService.instance.accessToken!,
      conversationId: widget.conversationId,
      onMessage: (message) {
        setState(() {
          final current = _conversation;
          if (current == null) {
            return;
          }

          _conversation = ConversationDetail(
            id: current.id,
            offerId: current.offerId,
            buyerId: current.buyerId,
            sellerId: current.sellerId,
            lastMessage: message.content,
            lastMessageAt: message.createdAt,
            unreadCount: current.unreadCount,
            status: current.status,
            createdAt: current.createdAt,
            messages: [...current.messages, message],
          );
        });
      },
    );

    return conversation;
  }

  @override
  void initState() {
    super.initState();
    _futureConversation = _loadConversation();
  }

  @override
  void dispose() {
    _realtimeService.disconnect(widget.conversationId);
    _controller.dispose();
    super.dispose();
  }

  Future<void> _sendMessage() async {
    final content = _controller.text.trim();

    if (content.isEmpty || _sending) {
      return;
    }

    setState(() {
      _sending = true;
    });

    try {
      await _conversationService.postNewConversationMessage(
        widget.conversationId,
        content,
      );

      _controller.clear();
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text(e.toString())));
    } finally {
      if (mounted) {
        setState(() => _sending = false);
      }
    }
  }

  Widget _messageWidget(Message message) {
    bool isMine =
        (_conversation == null) || message.senderId == _conversation!.buyerId;

    return Align(
      alignment: isMine ? Alignment.centerRight : Alignment.centerLeft,
      child: Container(
        margin: const EdgeInsets.only(bottom: 10),
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
        decoration: BoxDecoration(borderRadius: BorderRadius.circular(16)),
        child: Container(
          constraints: const BoxConstraints(maxWidth: 280),
          margin: const EdgeInsets.only(bottom: 10),
          padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
          decoration: BoxDecoration(
            color: isMine
                ? Theme.of(context).colorScheme.primary
                : Theme.of(context).colorScheme.surfaceContainerHighest,
            borderRadius: BorderRadius.only(
              topLeft: const Radius.circular(16),
              topRight: const Radius.circular(16),
              bottomLeft: Radius.circular(isMine ? 16 : 4),
              bottomRight: Radius.circular(isMine ? 4 : 16),
            ),
          ),
          child: Text(
            message.content,
            style: TextStyle(
              color: isMine
                  ? Colors.white
                  : Theme.of(context).colorScheme.onSurface,
              fontSize: 15,
              height: 1.3,
            ),
          ),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder(
      future: _futureConversation,
      builder: (context, snapshot) {
        final conversation = _conversation ?? snapshot.data;

        if (snapshot.connectionState == ConnectionState.waiting &&
            conversation == null) {
          return const Scaffold(
            body: Center(child: CircularProgressIndicator()),
          );
        }

        if (snapshot.hasError && conversation == null) {
          return const Scaffold(
            body: Center(child: Text('Błąd pobrania rozmowy!')),
          );
        }

        if (conversation == null) {
          return const Scaffold(body: Center(child: Text('Brak rozmowy')));
        }

        return Scaffold(
          appBar: MainAppBar(),
          body: Column(
            children: [
              Expanded(
                child: ListView.builder(
                  padding: const EdgeInsets.all(16),
                  itemCount: conversation.messages.length,
                  itemBuilder: (context, index) {
                    final message = conversation.messages[index];
                    return _messageWidget(message);
                  },
                ),
              ),
              SafeArea(
                child: Padding(
                  padding: const EdgeInsets.all(12),
                  child: Row(
                    children: [
                      Expanded(
                        child: TextField(
                          controller: _controller,
                          minLines: 1,
                          maxLines: 4,
                          decoration: const InputDecoration(
                            hintText: 'Napisz wiadomość...',
                            border: OutlineInputBorder(),
                            fillColor: Colors.white54,
                          ),
                        ),
                      ),
                      const SizedBox(width: 8),
                      IconButton(
                        onPressed: _sending ? null : _sendMessage,
                        icon: _sending
                            ? const SizedBox(
                                width: 22,
                                height: 22,
                                child: CircularProgressIndicator(
                                  strokeWidth: 2,
                                ),
                              )
                            : const Icon(Icons.send),
                      ),
                    ],
                  ),
                ),
              ),
            ],
          ),
        );
      },
    );
  }
}
