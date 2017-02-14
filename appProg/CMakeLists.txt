cmake_minimum_required(VERSION 3.6)
project(1)

set(CMAKE_CXX_STANDARD 11)

find_package(PkgConfig REQUIRED)
pkg_search_module(GTK REQUIRED gtk+-3.0)
include_directories(${GTK_INCLUDE_DIRS})
link_directories(${GTK_LIBRARY_DIRS})

set(SOURCE_FILES src/app.cpp)
add_executable(1 ${SOURCE_FILES})
target_link_libraries(1 ${GTK_LIBRARIES})
