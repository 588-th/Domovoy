shader_type spatial;
render_mode unshaded;

uniform sampler2D LIGHT_TEXTURE : hint_screen_texture, filter_linear_mipmap;
uniform sampler2D DEPTH_TEXTURE : hint_depth_texture, filter_linear_mipmap;

uniform float edge_threshold = 0.001;
uniform float depth_threshold = 10;
uniform vec3 line_color : source_color = vec3(0.200, 0.0, 0.24);
uniform float line_alpha = 0.5;
uniform float light_influence = 150;

const mat3 sobel_y = mat3(
    vec3(1.0, 0.0, -1.0),
    vec3(2.0, 0.0, -2.0),
    vec3(1.0, 0.0, -1.0)
);

const mat3 sobel_x = mat3(
    vec3(1.0, 2.0, 1.0),
    vec3(0.0, 0.0, 0.0),
    vec3(-1.0, -2.0, -1.0)
);

void vertex() {
    POSITION = vec4(VERTEX.xy, 1.0, 1.0);
}

void fragment() {
    vec2 uv = SCREEN_UV;
    vec2 offset = 0.5 / VIEWPORT_SIZE;

    float dn = texture(DEPTH_TEXTURE, uv + vec2(0.0, -offset.y)).r;
    float ds = texture(DEPTH_TEXTURE, uv + vec2(0.0, offset.y)).r;
    float de = texture(DEPTH_TEXTURE, uv + vec2(offset.x, 0.0)).r;
    float dw = texture(DEPTH_TEXTURE, uv + vec2(-offset.x, 0.0)).r;
    float dnw = texture(DEPTH_TEXTURE, uv + vec2(-offset.x, -offset.y)).r;
    float dne = texture(DEPTH_TEXTURE, uv + vec2(offset.x, -offset.y)).r;
    float dsw = texture(DEPTH_TEXTURE, uv + vec2(-offset.x, offset.y)).r;
    float dse = texture(DEPTH_TEXTURE, uv + vec2(offset.x, offset.y)).r;
    
    float center_depth = texture(DEPTH_TEXTURE, uv).r;

    mat3 depth_differences = mat3(
        vec3(abs(dnw - center_depth), abs(dn - center_depth), abs(dne - center_depth)),
        vec3(abs(dw - center_depth), 0.0, abs(de - center_depth)),
        vec3(abs(dsw - center_depth), abs(ds - center_depth), abs(dse - center_depth))
    );

    float depth_x = dot(sobel_x[0], depth_differences[0]) + dot(sobel_x[1], depth_differences[1]) + dot(sobel_x[2], depth_differences[2]);
    float depth_y = dot(sobel_y[0], depth_differences[0]) + dot(sobel_y[1], depth_differences[1]) + dot(sobel_y[2], depth_differences[2]);
    float depth_edge = sqrt(depth_x * depth_x + depth_y * depth_y);

    float edge = depth_edge * depth_threshold;

    float normalized_edge = edge / max(0.01, edge_threshold);

    float light_intensity = texture(LIGHT_TEXTURE, uv).r;
    float adjusted_alpha = mix(line_alpha, 0.0, light_intensity * light_influence);

    if (adjusted_alpha < 0.0) {
        adjusted_alpha = 0.0;
    }

    if (normalized_edge > edge_threshold) {
        ALBEDO = line_color;
        ALPHA = adjusted_alpha;
    } else {
        discard;
    }
}
