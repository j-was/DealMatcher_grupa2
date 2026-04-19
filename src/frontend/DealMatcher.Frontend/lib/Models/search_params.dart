class SearchParams {
  final int? _categoryId;
  final double? _minPrice;
  final double? _maxPrice;
  final List<String>? _tags;
  final Map<String, List<String>>? _properties;
  final String? _searchPhrase;
  final int _limit;

  int? get categoryId => _categoryId;
  double? get minPrice => _minPrice;
  double? get maxPrice => _maxPrice;
  List<String>? get tags => _tags;
  Map<String, List<String>>? get properties => _properties;
  String? get searchPhrase => _searchPhrase;
  int get limit => _limit;

  const SearchParams({
    int? categoryId,
    double? minPrice,
    double? maxPrice,
    List<String>? tags,
    Map<String, List<String>>? properties,
    String? searchPhrase,
    required int limit,
  }) : _categoryId = categoryId,
       _minPrice = minPrice,
       _maxPrice = maxPrice,
       _tags = tags,
       _properties = properties,
       _searchPhrase = searchPhrase,
       _limit = limit;

  Map<String, dynamic> toJson() => {
    'categoryId': _categoryId,
    'minPrice': _minPrice,
    'maxPrice': _maxPrice,
    'tags': _tags,
    'properties': _properties,
    'searchPhrase': _searchPhrase,
    'limit': _limit,
  };
}
