FROM postgres:latest
USER postgres

# Copy postgres config file into container
COPY postgresql1.conf /etc/postgresql

# Override default postgres config file
CMD ["postgres", "-c", "config_file=/etc/postgresql/postgresql.conf"]