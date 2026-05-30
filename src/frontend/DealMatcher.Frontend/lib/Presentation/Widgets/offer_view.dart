import 'package:flutter/material.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Services/conversation_service.dart';
import 'package:go_router/go_router.dart';

class OfferView extends StatefulWidget {
  final Offer offer;

  const OfferView({super.key, required this.offer});

  @override
  State createState() => OfferViewState();
}

class OfferViewState extends State<OfferView> {
  bool ifExpanded = false;
  bool _conversationLoading = false;
  final ConversationService _conversationService = ConversationService();
  int _imageIndex = 0;

  Future<void> _startConversation() async {
    final controller = TextEditingController();

    final message = await showDialog<String>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Napisz do autora oferty'),
        content: TextField(controller: controller, minLines: 3, maxLines: 10),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Anuluj'),
          ),
          ElevatedButton(
            onPressed: () {
              final text = controller.text.trim();
              if (text.isEmpty) {
                return;
              }

              Navigator.pop(context, text);
            },
            child: const Text('Wyślij'),
          ),
        ],
      ),
    );

    if (message == null || message.isEmpty) {
      return;
    }

    setState(() {
      _conversationLoading = true;
    });

    try {
      final conversation = await _conversationService.createConversation(
        offerId: widget.offer.id,
        initialMessage: message,
      );

      if (!mounted) {
        return;
      }

      context.push('conversations/${conversation.id}');
    } catch (e) {
      if (!mounted) {
        return;
      }

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text(e.toString())));
    } finally {
      if (mounted) {
        setState(() {
          _conversationLoading = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final hasImage = widget.offer.images.isNotEmpty;

    return Center(
      child: SizedBox(
        width: 400,
        height: ifExpanded ? 610 : 425,
        child: InkWell(
          onTap: () {
            setState(() {
              ifExpanded = !ifExpanded;
            });
          },
          child: Card(
            clipBehavior: Clip.antiAlias,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadiusGeometry.circular(16),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                if (hasImage)
                  SizedBox(
                    height: 220,
                    width: double.infinity,
                    child: Stack(
                      children: [
                        Positioned.fill(
                          child: Container(
                            color: Colors.black12,
                            child: Center(
                              child: Image.network(
                                widget.offer.images[_imageIndex],
                                fit: BoxFit.contain,
                                width: double.infinity,
                                height: double.infinity,
                                errorBuilder: (context, error, stackTrace) {
                                  return Container(
                                    color: Colors.grey.shade300,
                                    child: const Center(
                                      child: Icon(Icons.broken_image_outlined),
                                    ),
                                  );
                                },
                              ),
                            ),
                          ),
                        ),

                        if (widget.offer.images.length > 1) ...[
                          Positioned(
                            left: 8,
                            top: 0,
                            bottom: 0,
                            child: Center(
                              child: IconButton(
                                style: IconButton.styleFrom(
                                  backgroundColor: Colors.black45,
                                  foregroundColor: Colors.white,
                                ),
                                onPressed: _imageIndex > 0
                                    ? () {
                                        setState(() {
                                          _imageIndex--;
                                        });
                                      }
                                    : null,
                                icon: const Icon(Icons.chevron_left),
                              ),
                            ),
                          ),

                          Positioned(
                            right: 8,
                            top: 0,
                            bottom: 0,
                            child: Center(
                              child: IconButton(
                                style: IconButton.styleFrom(
                                  backgroundColor: Colors.black45,
                                  foregroundColor: Colors.white,
                                ),
                                onPressed:
                                    _imageIndex < widget.offer.images.length - 1
                                    ? () {
                                        setState(() {
                                          _imageIndex++;
                                        });
                                      }
                                    : null,
                                icon: const Icon(Icons.chevron_right),
                              ),
                            ),
                          ),

                          Positioned(
                            bottom: 8,
                            left: 0,
                            right: 0,
                            child: Row(
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: List.generate(
                                widget.offer.images.length,
                                (index) => Container(
                                  width: 8,
                                  height: 8,
                                  margin: const EdgeInsets.symmetric(
                                    horizontal: 3,
                                  ),
                                  decoration: BoxDecoration(
                                    shape: BoxShape.circle,
                                    color: index == _imageIndex
                                        ? Colors.white
                                        : Colors.white54,
                                  ),
                                ),
                              ),
                            ),
                          ),
                        ],
                      ],
                    ),
                  ),
                Expanded(
                  child: ListView(
                    children: [
                      Padding(
                        padding: const EdgeInsets.all(12),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Row(
                              children: [
                                Expanded(
                                  child: Text(
                                    widget.offer.title,
                                    style: const TextStyle(
                                      fontWeight: FontWeight.bold,
                                      fontSize: 20,
                                    ),
                                  ),
                                ),
                                IconButton(
                                  onPressed: _conversationLoading
                                      ? null
                                      : _startConversation,
                                  icon: _conversationLoading
                                      ? const SizedBox(
                                          width: 18,
                                          height: 18,
                                          child: CircularProgressIndicator(
                                            strokeWidth: 2,
                                          ),
                                        )
                                      : const Icon(
                                          Icons.chat_bubble_outline,
                                          color: Colors.black87,
                                        ),
                                ),
                              ],
                            ),
                            Text('${widget.offer.price.toStringAsFixed(2)} zł'),
                            const SizedBox(height: 12),
                            Wrap(
                              spacing: 6,
                              children: widget.offer.tags
                                  .map((tag) => Chip(label: Text(tag)))
                                  .toList(),
                            ),
                            const SizedBox(height: 8),
                            Text(
                              widget.offer.description,
                              maxLines: ifExpanded ? null : 2,
                              overflow: ifExpanded
                                  ? TextOverflow.visible
                                  : TextOverflow.ellipsis,
                            ),
                          ],
                        ),
                      ),
                      if (ifExpanded) ...[
                        Padding(
                          padding: const EdgeInsets.fromLTRB(12, 0, 12, 12),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const SizedBox(height: 8),
                              ...widget.offer.properties.map(
                                (property) => Row(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text('${property.$1}: '),
                                    Expanded(child: Text(property.$2)),
                                  ],
                                ),
                              ),
                              const SizedBox(height: 8),
                              Text('Ilość: ${widget.offer.availability}'),
                              const SizedBox(height: 8),
                              Text(
                                'Utworzono: ${widget.offer.createdAt.toString().split(' ')[0]}',
                                style: const TextStyle(fontSize: 12),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ],
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
