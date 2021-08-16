FROM node:lts as builder
COPY package.json package-lock.json ./
RUN npm install && mkdir /builder && mv ./node_modules ./builder
WORKDIR /builder
COPY . .
ARG FRONTEND_CONFIGURATION=development
RUN npm run build -- --configuration=$FRONTEND_CONFIGURATION

FROM nginx:alpine
COPY nginx.conf /etc/nginx/nginx.conf
RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /builder/dist/* /usr/share/nginx/html
EXPOSE 80 443
ENTRYPOINT ["nginx", "-g", "daemon off;"]
