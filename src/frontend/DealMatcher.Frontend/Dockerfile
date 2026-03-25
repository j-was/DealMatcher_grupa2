FROM ghcr.io/cirruslabs/flutter:stable AS build

WORKDIR /frontend
COPY . .

RUN flutter pub get

ARG API_URL
RUN flutter build web --release --dart-define=API_URL=${API_URL}

FROM nginx:alpine
COPY --from=build /frontend/build/web /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]