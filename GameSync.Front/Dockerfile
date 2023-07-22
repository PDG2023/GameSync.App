FROM node:slim as node
WORKDIR /app
COPY package*.json .
RUN npm install
RUN npm run ng build

FROM nginx:alpine
COPY ./nginx.conf /etc/nginx/nginx.conf
COPY --from=node /app/dist/* /usr/share/nginx/html
